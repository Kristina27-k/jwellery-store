using JewelryStore.Api.Models.DTO;

namespace JewelryStore.Api.Services;

public interface IJewelryService
{
    Task<IEnumerable<JewelryItemDto>> GetAllJewelryAsync();
    Task<JewelryItemDto?> GetJewelryByIdAsync(int id);
    Task AddJewelryAsync(JewelryItemDto itemDto);
    Task UpdateJewelryAsync(JewelryItemDto itemDto);
    Task DeleteJewelryAsync(int id);
}
