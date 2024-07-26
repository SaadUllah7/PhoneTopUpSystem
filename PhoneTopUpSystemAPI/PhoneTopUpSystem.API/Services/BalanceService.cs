namespace PhoneTopUpSystem.API;

public class BalanceService : IBalanceService
{
    // private readonly string balanceServiceUrl = Configuration.
    private readonly HttpClient _httpClient;

    public BalanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetBalanceAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5045/api/balance/{userId}");
        response.EnsureSuccessStatusCode();
        var balance = await response.Content.ReadAsAsync<decimal>();
        return balance;
    }

    public async Task<bool> DebitBalanceAsync(int userId, decimal amount)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5045/api/balance/debit", new { UserId = userId, Amount = amount });
        return response.IsSuccessStatusCode;
    }
}
