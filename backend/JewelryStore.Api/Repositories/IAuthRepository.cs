using JewelryStore.Api.Models.Entities;

namespace JewelryStore.Api.Repositories;

public interface IAuthRepository
{
    Task<UserEntity?> GetByEmailAsync(string email);
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<int> CreateAsync(UserEntity user);
    Task<bool> ExistsAsync(string email);
}
