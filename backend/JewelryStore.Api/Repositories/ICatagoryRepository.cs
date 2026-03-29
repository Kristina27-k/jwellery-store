using JewelryStore.Api.Models.Entities;

namespace JewelryStore.Api.Repositories;

public interface ICategoryRepository
{
    Task<CategoryEntity?> GetAllCatagoryAsync();
}
