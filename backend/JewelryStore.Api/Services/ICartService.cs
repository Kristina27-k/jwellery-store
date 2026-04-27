using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface ICartService
{
    Task<ServiceResponse<IEnumerable<CartItemDto>>> GetUserCartAsync(int userId);
    Task<ServiceResponse<CartItemDto>> AddToCartAsync(int userId, int itemId, int quantity);
    Task<ServiceResponse<bool>> UpdateCartItemQuantityAsync(int cartItemId, int quantity);
    Task<ServiceResponse<bool>> RemoveFromCartAsync(int cartItemId);
    Task<ServiceResponse<bool>> ClearUserCartAsync(int userId);
}
