using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface ICategoryService
{
    Task<ServiceResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
    Task<ServiceResponse<CategoryDto>> GetCategoryByIdAsync(int id);
    Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto);
    Task<ServiceResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto);
    Task<ServiceResponse<bool>> DeleteCategoryAsync(int id);
    Task<ServiceResponse<CategoryDto>> GetAllCategoryAsync();
}
