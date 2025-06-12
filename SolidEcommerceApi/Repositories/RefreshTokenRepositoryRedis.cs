using SolidEcommerceApi.Models;
using SolidEcommerceApi.Services;

namespace SolidEcommerceApi.Repositories;

public class RefreshTokenRepositoryRedis : IRefreshTokenRepository
{
    private readonly IRedisService _redis;

    public RefreshTokenRepositoryRedis(IRedisService redis)
    {
        _redis = redis;
    }

    public async Task AddAsync(RefreshToken token)
    {
        var expiry = token.ExpiryDate - DateTime.UtcNow;
        await _redis.SetAsync($"refresh:{token.Token}", token.Username, expiry);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var username = await _redis.GetAsync($"refresh:{token}");
        return username == null ? null : new RefreshToken
        {
            Token = token,
            Username = username
        };
    }

    public async Task DeleteAsync(RefreshToken token) =>
        await _redis.RemoveAsync($"refresh:{token.Token}");

    public Task DeleteAllAsync(string username)
    {
        // Redis tarafında kullanıcıya bağlı tüm refresh token’lar bulk silinemez
        // Redis key'leri listelemek için ayrı strateji gerekir
        return Task.CompletedTask;
    }
}
