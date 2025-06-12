using SolidEcommerceApi.DTOs;
using SolidEcommerceApi.Models;
using SolidEcommerceApi.Repositories;

namespace SolidEcommerceApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<User>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<User?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _repository.GetByUsernameAsync(username);


    
    public async Task<User> CreateAsync(User dto)
    {
        var user = new User { Username = dto.Username, PasswordHash = dto.PasswordHash};
        await _repository.AddAsync(user);
        return user;
    }

    public async Task<User?> UpdateAsync(int id, UserDto dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        user.Username = dto.Username;
        await _repository.UpdateAsync(user);
        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return false;

        await _repository.DeleteAsync(user);
        return true;
    }
}
