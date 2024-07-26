using Microsoft.AspNetCore.Http.HttpResults;
using PhoneTopUpSystem.API;

namespace PhoneTopUpSystem.Tests;

public class BalanceServiceMock : IBalanceService
{

    List<UserBalanceData> balanceData = new List<UserBalanceData>();

    public BalanceServiceMock()
    {
        balanceData = new List<UserBalanceData> {
            new UserBalanceData {
                    UserId = 1,
                    Balance = 1000
                },
            new UserBalanceData {
                    UserId = 2, 
                    Balance = 1000
                }
            };
    }

    public Task<bool> DebitBalanceAsync(int userId, decimal amount)
    {
        var user = balanceData.FirstOrDefault(x => x.UserId == userId);
        user.Balance = user.Balance - amount;
        return Task.FromResult(true);
    }

    public Task<decimal> GetBalanceAsync(int userId)
    {
        var user = balanceData.FirstOrDefault(x => x.UserId == userId);
        return Task.FromResult(user.Balance);
    }
}
