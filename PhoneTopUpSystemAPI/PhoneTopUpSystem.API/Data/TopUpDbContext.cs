using Microsoft.EntityFrameworkCore;

namespace PhoneTopUpSystem.API;

public class TopUpDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Beneficiary> Beneficiaries { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public TopUpDbContext(DbContextOptions<TopUpDbContext> options) : base(options)
    {
    }
}

