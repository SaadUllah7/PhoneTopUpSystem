using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PhoneTopUpSystem.API;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly TopUpDbContext _dbContext;

    public BeneficiaryService(TopUpDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Beneficiary>> GetBeneficiaries(int userId)
    {
       return  await _dbContext.Beneficiaries.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task<Beneficiary> AddBeneficiary(int userId, string nickname)
    {
        try
        {
            var user = await _dbContext.Users.Include(u => u.Beneficiaries).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException($"User with Id {userId} not found.");

            if (user.Beneficiaries.Count >= 5)
                throw new BadHttpRequestException($"Cannot add more than 5 beneficiaries for user {user.Id}-{user.UserName}.");

            if(user.Beneficiaries.Any(b => b.Nickname.ToLower() == nickname.ToLower())){
                throw new DuplicateNameException("Beneficiary already exists with this name.");
            }

            var beneficiary = new Beneficiary
            {
                UserId = userId,
                Nickname = nickname,
            };

            _dbContext.Beneficiaries.Add(beneficiary);
            await _dbContext.SaveChangesAsync();

            return beneficiary;

        }
        catch (Exception)
        {
            throw;
        }
    }
}
