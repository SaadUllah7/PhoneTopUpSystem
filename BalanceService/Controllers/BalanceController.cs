using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BalanceService;

[ApiController]
[Route("api/[controller]")]
public class BalanceController : ControllerBase
{
    private readonly BalanceServiceDbContext _dbContext;

    public BalanceController(BalanceServiceDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBalance(int userId)
    {
        // var user = await _dbContext.Users.FindAsync(userId);
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TopUpUserId == userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user.Balance);
    }

    [HttpPost("debit")]
    public async Task<IActionResult> DebitBalance([FromBody] BalanceTransactionRequest request)
    {
        // var user = await _dbContext.Users.FindAsync(request.UserId);
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TopUpUserId == request.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.Balance < request.Amount)
        {
            return BadRequest("Insufficient balance.");
        }

        user.Balance -= request.Amount;
        await _dbContext.SaveChangesAsync();

        return Ok(true);
    }

    [HttpPost("credit")]
    public async Task<IActionResult> CreditBalance([FromBody] BalanceTransactionRequest request)
    {
        // var user = await _dbContext.Users.FindAsync(request.UserId);
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.TopUpUserId == request.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Balance += request.Amount;
        await _dbContext.SaveChangesAsync();

        return Ok(true);
    }
}
