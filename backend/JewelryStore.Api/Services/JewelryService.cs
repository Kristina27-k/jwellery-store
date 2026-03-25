using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Entities;
using JewelryStore.Api.Repositories;

namespace JewelryStore.Api.Services;

public class JewelryService : IJewelryService
{
    private readonly IJewelryRepository _repository;

    public JewelryService(IJewelryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<IEnumerable<JewelryItemDto>>> GetAllJewelryAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = entities.Select(MapToDto);
        return ServiceResponse<IEnumerable<JewelryItemDto>>.Success(dtos);
    }

    public async Task<ServiceResponse<JewelryItemDto>> GetJewelryByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return ServiceResponse<JewelryItemDto>.Failure("Product not found.");
        return ServiceResponse<JewelryItemDto>.Success(MapToDto(entity));
    }

    public async Task<ServiceResponse<JewelryItemDto>> AddJewelryAsync(JewelryItemDto itemDto)
    {
        var entity = MapToEntity(itemDto);
        await _repository.AddAsync(entity);
        itemDto.Id = entity.Id;
        return ServiceResponse<JewelryItemDto>.Success(itemDto, "Product added successfully.");
    }

    public async Task<ServiceResponse<bool>> UpdateJewelryAsync(JewelryItemDto itemDto)
    {
        var entity = MapToEntity(itemDto);
        await _repository.UpdateAsync(entity);
        return ServiceResponse<bool>.Success(true, "Product updated successfully.");
    }

    public async Task<ServiceResponse<bool>> DeleteJewelryAsync(int id)
    {
        await _repository.DeleteAsync(id);
        return ServiceResponse<bool>.Success(true, "Product deleted successfully.");
    }

    private static JewelryItemDto MapToDto(JewelryItemEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Price = entity.Price,
        ImageUrl = entity.ImageUrl,
        CategoryId = entity.CategoryId
    };

    private static JewelryItemEntity MapToEntity(JewelryItemDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Description = dto.Description,
        Price = dto.Price,
        ImageUrl = dto.ImageUrl,
        CategoryId = dto.CategoryId
    };
}
