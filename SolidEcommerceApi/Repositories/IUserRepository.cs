using SolidEcommerceApi.Models;

namespace SolidEcommerceApi.Repositories;


public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User?> GetByUsernameAsync(string userName);

}