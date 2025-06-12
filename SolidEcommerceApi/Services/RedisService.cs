namespace SolidEcommerceApi.Services;

using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;
    private const int MaxRetries = 3;
    private const int ExpirationMinutes = 60;
    
    public RedisService(IConfiguration configuration)
    {
        var connection = ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]);
        _db = connection.GetDatabase();
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry = null) =>
        await _db.StringSetAsync(key, value, expiry);

    public async Task<string?> GetAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task RemoveAsync(string key) =>
        await _db.KeyDeleteAsync(key);
    
    public async Task<bool> IsLimitExceededAsync(string refreshToken)
    {
        var key = $"refresh:{refreshToken}";
        var count = await _db.StringIncrementAsync(key);

        if (count == 1)
            await _db.KeyExpireAsync(key, TimeSpan.FromMinutes(ExpirationMinutes));

        return count > MaxRetries;
    }
}
