using Microsoft.AspNetCore.Mvc;

namespace PhoneTopUpSystem.API;

public interface ITopUpService
{
    Task PerformTopUp(int userId, int beneficiaryId, decimal amount);
}
