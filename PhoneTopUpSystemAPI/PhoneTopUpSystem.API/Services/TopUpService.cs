using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace PhoneTopUpSystem.API;
public class TopUpService : ITopUpService
{

    private readonly TopUpDbContext _dbContext;
    private readonly IBalanceService _balanceService;

    public TopUpService(TopUpDbContext dbContext, IBalanceService balanceService)
    {
        _dbContext = dbContext;
        _balanceService = balanceService;
    }

    public async Task PerformTopUp(int userId, int beneficiaryId, decimal amount)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var beneficiary = await _dbContext.Beneficiaries.FindAsync(beneficiaryId);

            if (user == null || beneficiary == null || beneficiary.UserId != userId)
                throw new KeyNotFoundException("User or beneficiary not found.");

            if (amount <= 0)
                throw new BadHttpRequestException("Invalid top-up amount.");

            decimal monthlyLimitPerBeneficiaryPerMonth = user.IsVerified ? TopUpLimits.unVerifiedBeneficiaryTopUpLimitPerMonth : TopUpLimits.verifiedBeneficiaryTopUpLimitPerMonth;
            decimal monthlyLimitPerUserPerMonth = TopUpLimits.topUpLimitPerUserPerMonth;
            decimal transactionCharges = 1;
            decimal totalTopUpThisMonthPerUser = await GetTotalTopUpAmountForUserThisMonth(userId);
            decimal totalTopUpThisMonthPerBeneficiary = await GetTotalTopUpAmountForBeneficiaryThisMonth(userId, beneficiaryId);

            if (totalTopUpThisMonthPerBeneficiary + amount > monthlyLimitPerBeneficiaryPerMonth)
                throw new Exception($"Monthly limit exceeded for this beneficiary.");


            if (totalTopUpThisMonthPerUser + amount > monthlyLimitPerUserPerMonth)
                throw new Exception($"Monthly limit exceeded of this user for all beneficiaries.");

            decimal balance = 0;
            try
            {
                balance = await _balanceService.GetBalanceAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: Unable to retrieve balance information. Please ensure the Balance Service is running and the user credentials are correct.", ex);
            }

            if (balance < amount + transactionCharges)
                throw new Exception("Insufficient balance.");

            if (!await _balanceService.DebitBalanceAsync(userId, amount + transactionCharges))
                throw new Exception("Failed to debit balance.");


            var transaction = new Transaction
            {
                UserId = userId,
                BeneficiaryId = beneficiaryId,
                Amount = amount,
                TransactionDate = DateTime.UtcNow
            };

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<decimal> GetTotalTopUpAmountForUserThisMonth(int userId)
    {
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var totalTopUp = await _dbContext.Transactions
            .Where(t => t.UserId == userId && t.TransactionDate.Month == currentMonth && t.TransactionDate.Year == currentYear)
            .SumAsync(t => t.Amount);

        return totalTopUp;
    }

    private async Task<decimal> GetTotalTopUpAmountForBeneficiaryThisMonth(int userId, int beneficiaryId)
    {
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var totalTopUp = await _dbContext.Transactions
            .Where(t => t.UserId == userId && t.BeneficiaryId == beneficiaryId && t.TransactionDate.Month == currentMonth && t.TransactionDate.Year == currentYear)
            .SumAsync(t => t.Amount);

        return totalTopUp;
    }
}
