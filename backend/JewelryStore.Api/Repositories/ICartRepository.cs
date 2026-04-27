using JewelryStore.Api.Models.Entities;

namespace JewelryStore.Api.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<dynamic>> GetByUserIdAsync(int userId);
    Task<int> AddItemAsync(CartItemEntity item);
    Task UpdateQuantityAsync(int id, int quantity);
    Task RemoveItemAsync(int id);
    Task ClearCartAsync(int userId);
    Task<CartItemEntity?> GetByUserIdAndItemIdAsync(int userId, int itemId);
}
