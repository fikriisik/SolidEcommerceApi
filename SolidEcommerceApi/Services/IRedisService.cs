namespace SolidEcommerceApi.Services;
using System.Threading.Tasks;

public interface IRedisService
{
    Task SetAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
    Task<bool> IsLimitExceededAsync(string refreshToken);

}