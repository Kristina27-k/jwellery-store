using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface ICategoryService
{
    
    Task<ServiceResponse<CategoryDto>> GetAllCategoryAsync();
}
