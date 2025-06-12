using SolidEcommerceApi.DTOs;
using SolidEcommerceApi.Models;

namespace SolidEcommerceApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User dto);
    Task<User?> UpdateAsync(int id, UserDto dto);
    Task<bool> DeleteAsync(int id);
    Task<User?> GetByUsernameAsync(string userName);

}