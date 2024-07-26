using Microsoft.AspNetCore.Mvc;

namespace PhoneTopUpSystem.API;

[GlobalExceptionHandler]
[ApiController]
[Route("api/[controller]")]
public class BeneficiaryController : ControllerBase
{
    private readonly IBeneficiaryService _beneficiaryService;
    private readonly IBalanceService _balanceService;

    public BeneficiaryController(IBeneficiaryService benificiaryService, IBalanceService balanceService)
    {
        _beneficiaryService = benificiaryService;
        _balanceService = balanceService;
    }

    [HttpGet("beneficiaries/{userId}")]
    public async Task<IActionResult> GetBeneficiaries(int userId)
    {
        var beneficiaries = await _beneficiaryService.GetBeneficiaries(userId);
        
        var result = beneficiaries.Select(b => new BeneficiaryDTO{
            Id= b.Id,
            UserId = b.UserId,
            Nickname = b.Nickname
        }).ToList();

        return Ok(result);
    }

    [HttpPost("addbeneficiary")]
    public async Task<IActionResult> AddBeneficiary(int userId, string nickname)
    {
        if (nickname.Length > 20)
            return BadRequest("Nickname cannot exceed 20 characters.");

        var beneficiary =  await _beneficiaryService.AddBeneficiary(userId, nickname);
        var dto = new BeneficiaryDTO{
            Id= beneficiary.Id,
            UserId = beneficiary.UserId,
            Nickname = beneficiary.Nickname
        };
        return Ok(dto);
    }
}
