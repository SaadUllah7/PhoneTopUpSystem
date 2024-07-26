using Microsoft.EntityFrameworkCore;

namespace BalanceService;

public class BalanceServiceDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public BalanceServiceDbContext(DbContextOptions<BalanceServiceDbContext> options) : base(options)
    {
    }
}


