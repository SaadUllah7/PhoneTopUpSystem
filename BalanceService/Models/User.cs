namespace BalanceService;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int TopUpUserId { get; set; }
    public decimal Balance { get; set; }
}

