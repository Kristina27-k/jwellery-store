using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Entities;
using JewelryStore.Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace JewelryStore.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IConfiguration _configuration;

    public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration)
    {
        _categoryRepository = categoryRepository;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = categories.Select(c => MapToDto(c)).ToList();
        return ServiceResponse<IEnumerable<CategoryDto>>.Success(dtos, "Categories retrieved successfully.");
    }

    public async Task<ServiceResponse<CategoryDto>> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            return ServiceResponse<CategoryDto>.Failure("Category not found.");

        return ServiceResponse<CategoryDto>.Success(MapToDto(category), "Category retrieved successfully.");
    }

    public async Task<ServiceResponse<CategoryDto>> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var entity = new CategoryEntity { cat_name = categoryDto.Name, Description = categoryDto.Description };
        var id = await _categoryRepository.CreateAsync(entity);
        categoryDto.Id = id;
        return ServiceResponse<CategoryDto>.Success(categoryDto, "Category created successfully.");
    }

    public async Task<ServiceResponse<bool>> UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var entity = new CategoryEntity { Id = categoryDto.Id, cat_name = categoryDto.Name, Description = categoryDto.Description };
        await _categoryRepository.UpdateAsync(entity);
        return ServiceResponse<bool>.Success(true, "Category updated successfully.");
    }

    public async Task<ServiceResponse<bool>> DeleteCategoryAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
        return ServiceResponse<bool>.Success(true, "Category deleted successfully.");
    }

    public async Task<ServiceResponse<CategoryDto>> GetAllCategoryAsync()
    {
        var result = await _categoryRepository.GetAllCatagoryAsync();
        if (result == null)
            return ServiceResponse<CategoryDto>.Failure("Category not found");

        var dto = new CategoryDto
        {
            Id = result.Id,
            Name = result.cat_name,
            Description = result.Description
        };

        return ServiceResponse<CategoryDto>.Success(dto, "Get successful.");
    }

    private static CategoryDto MapToDto(CategoryEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.cat_name,
        Description = entity.Description
    };
}
