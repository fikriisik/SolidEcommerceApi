namespace SolidEcommerceApi.Repositories;

public interface IRevokedTokenRepository
{
    Task AddAsync(string jti, DateTime expiry);
    Task<bool> IsRevokedAsync(string jti);
}