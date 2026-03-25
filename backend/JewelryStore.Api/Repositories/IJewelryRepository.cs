using JewelryStore.Api.Models.Entity;

namespace JewelryStore.Api.Repositories;

public interface IJewelryRepository
{
    Task<IEnumerable<JewelryItemEntity>> GetAllAsync();
    Task<JewelryItemEntity?> GetByIdAsync(int id);
    Task AddAsync(JewelryItemEntity item);
    Task UpdateAsync(JewelryItemEntity item);
    Task DeleteAsync(int id);
}
