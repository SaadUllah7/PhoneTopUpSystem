namespace PhoneTopUpSystem.API;

public interface IBalanceService
{
    Task<decimal> GetBalanceAsync(int userId);
    Task<bool> DebitBalanceAsync(int userId, decimal amount);
}
