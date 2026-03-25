using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface IJewelryService
{
    Task<ServiceResponse<IEnumerable<JewelryItemDto>>> GetAllJewelryAsync();
    Task<ServiceResponse<JewelryItemDto>> GetJewelryByIdAsync(int id);
    Task<ServiceResponse<JewelryItemDto>> AddJewelryAsync(JewelryItemDto itemDto);
    Task<ServiceResponse<bool>> UpdateJewelryAsync(JewelryItemDto itemDto);
    Task<ServiceResponse<bool>> DeleteJewelryAsync(int id);
}
