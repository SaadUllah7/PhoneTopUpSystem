using Microsoft.AspNetCore.Mvc;
using PhoneTopUpSystem.API;

public interface IBeneficiaryService{
    Task<List<Beneficiary>> GetBeneficiaries(int userId);
    Task<Beneficiary> AddBeneficiary(int userId, string nickname);
}