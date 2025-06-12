using Microsoft.EntityFrameworkCore;
using SolidEcommerceApi.Data;
using SolidEcommerceApi.Models;

namespace SolidEcommerceApi.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token) =>
        await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

    public async Task DeleteAsync(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync(string username)
    {
        var tokens = _context.RefreshTokens.Where(x => x.Username == username);
        _context.RefreshTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }
}
