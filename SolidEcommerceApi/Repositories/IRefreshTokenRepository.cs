using SolidEcommerceApi.Models;

namespace SolidEcommerceApi.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task DeleteAsync(RefreshToken token);
    Task DeleteAllAsync(string username);
    
}