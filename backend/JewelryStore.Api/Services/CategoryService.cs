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
    private readonly ICategoryRepository _catrepo;
    private readonly IConfiguration _configuration;

    public CategoryService(ICategoryRepository CategoryRepository, IConfiguration configuration)
    {
        _catrepo = CategoryRepository;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<CategoryDto>> GetAllCategoryAsync(CategoryDto request)
    {
        var result = await _catrepo.GetAllCatagoryAsync();

        if (result == null)
        {
            return ServiceResponse<CategoryDto>.Failure("Category not found");
        }


        var dto = new CategoryDto
        {
            Id = result.Id,
            Name = result.cat_name,
            Description = result.Description
        };

        return ServiceResponse<CategoryDto>.Success(dto, "Get successful.");
    }
}
