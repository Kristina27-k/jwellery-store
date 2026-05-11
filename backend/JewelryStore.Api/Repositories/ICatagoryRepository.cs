using JewelryStore.Api.Models.Entities;

namespace JewelryStore.Api.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryEntity>> GetAllAsync();
    Task<CategoryEntity?> GetByIdAsync(int id);
    Task<int> CreateAsync(CategoryEntity category);
    Task UpdateAsync(CategoryEntity category);
    Task DeleteAsync(int id);
    Task<CategoryEntity?> GetAllCatagoryAsync();
}
