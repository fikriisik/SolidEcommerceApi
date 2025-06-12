using Microsoft.EntityFrameworkCore;
using SolidEcommerceApi.Data;
using SolidEcommerceApi.Models;
using SolidEcommerceApi.Services;

namespace SolidEcommerceApi.Repositories;

public class HybridRefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IRedisService _redis;
    private readonly AppDbContext _context;

    public HybridRefreshTokenRepository(IRedisService redis, AppDbContext context)
    {
        _redis = redis;
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        var expiry = token.ExpiryDate - DateTime.UtcNow;
        await _redis.SetAsync($"refresh:{token.Token}", token.Username, expiry);
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var username = await _redis.GetAsync($"refresh:{token}");
        if (username != null)
        {
            return new RefreshToken { Token = token, Username = username };
        }

        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task DeleteAsync(RefreshToken token)
    {
        await _redis.RemoveAsync($"refresh:{token.Token}");

        var dbToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token.Token);
        if (dbToken != null)
        {
            _context.RefreshTokens.Remove(dbToken);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync(string username)
    {
        var tokens = await _context.RefreshTokens
            .Where(t => t.Username == username).ToListAsync();

        foreach (var t in tokens)
        {
            await _redis.RemoveAsync($"refresh:{t.Token}");
        }

        _context.RefreshTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }
}
