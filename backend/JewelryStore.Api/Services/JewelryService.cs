using JewelryStore.Api.Models.DTO;
using JewelryStore.Api.Models.Entity;
using JewelryStore.Api.Repositories;

namespace JewelryStore.Api.Services;

public class JewelryService : IJewelryService
{
    private readonly IJewelryRepository _repository;

    public JewelryService(IJewelryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<JewelryItemDto>> GetAllJewelryAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(MapToDto);
    }

    public async Task<JewelryItemDto?> GetJewelryByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity != null ? MapToDto(entity) : null;
    }

    public async Task AddJewelryAsync(JewelryItemDto itemDto)
    {
        var entity = MapToEntity(itemDto);
        await _repository.AddAsync(entity);
        itemDto.Id = entity.Id;
    }

    public async Task UpdateJewelryAsync(JewelryItemDto itemDto)
    {
        var entity = MapToEntity(itemDto);
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteJewelryAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static JewelryItemDto MapToDto(JewelryItemEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Price = entity.Price,
        ImageUrl = entity.ImageUrl,
        CategoryId = entity.CategoryId
        // CategoryName would ideally be joined in the repository
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
