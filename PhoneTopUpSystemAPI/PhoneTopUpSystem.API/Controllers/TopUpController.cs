using Microsoft.AspNetCore.Mvc;

namespace PhoneTopUpSystem.API;


[GlobalExceptionHandler]
[ApiController]
[Route("api/[controller]")]
public class TopUpController : ControllerBase
{
    private readonly ITopUpService _topUpService;
    private readonly IBalanceService _balanceService;

    public TopUpController(ITopUpService topUpService, IBalanceService balanceService)
    {
        _topUpService = topUpService;
        _balanceService = balanceService;
    }

    [HttpGet("topupoptions")]
    public IActionResult GetTopUpOptions()
    {
        var options = new[] { 5, 10, 20, 30, 50, 75, 100 };
        return Ok(options);
    }

    [HttpPost]
    public async Task TopUp(int userId, int beneficiaryId, decimal amount)
    {

        await _topUpService.PerformTopUp(userId,beneficiaryId,amount);

        // return Ok(true);
    }
}


