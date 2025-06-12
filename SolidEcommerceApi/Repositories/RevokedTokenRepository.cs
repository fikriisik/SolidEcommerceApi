using Microsoft.EntityFrameworkCore;
using SolidEcommerceApi.Data;
using SolidEcommerceApi.Models;

namespace SolidEcommerceApi.Repositories;

public class RevokedTokenRepository : IRevokedTokenRepository
{
    private readonly AppDbContext _context;

    public RevokedTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(string jti, DateTime expiry)
    {
        await _context.RevokedTokens.AddAsync(new RevokedToken
        {
            Jti = jti,
            ExpiryDate = expiry
        });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsRevokedAsync(string jti)
    {
        return await _context.RevokedTokens.AnyAsync(x => x.Jti == jti && x.ExpiryDate > DateTime.UtcNow);
    }
}