using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Entities;
using JewelryStore.Api.Repositories;

namespace JewelryStore.Api.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repository;
    private readonly IJewelryRepository _itemRepository;

    public CartService(ICartRepository repository, IJewelryRepository itemRepository)
    {
        _repository = repository;
        _itemRepository = itemRepository;
    }

    public async Task<ServiceResponse<IEnumerable<CartItemDto>>> GetUserCartAsync(int userId)
    {
        var items = await _repository.GetByUserIdAsync(userId);
        var dtos = items.Select(i => new CartItemDto
        {
            Id = (int)i.id,
            JewelryItemId = (int)i.jewelryitemid,
            ProductName = (string)i.productname,
            Price = (decimal)i.price,
            ImageUrl = (string)i.imageurl,
            Quantity = (int)i.quantity
        });
        return ServiceResponse<IEnumerable<CartItemDto>>.Success(dtos);
    }

    public async Task<ServiceResponse<CartItemDto>> AddToCartAsync(int userId, int itemId, int quantity)
    {
        // Check if product exists
        var product = await _itemRepository.GetByIdAsync(itemId);
        if (product == null) return ServiceResponse<CartItemDto>.Failure("Product not found.");

        // Check if item already in cart
        var existingItem = await _repository.GetByUserIdAndItemIdAsync(userId, itemId);
        if (existingItem != null)
        {
            await _repository.UpdateQuantityAsync(existingItem.Id, existingItem.Quantity + quantity);
            return ServiceResponse<CartItemDto>.Success(new CartItemDto { Id = existingItem.Id }, "Quantity updated in cart.");
        }

        var cartItem = new CartItemEntity
        {
            UserId = userId,
            JewelryItemId = itemId,
            Quantity = quantity
        };

        cartItem.Id = await _repository.AddItemAsync(cartItem);
        return ServiceResponse<CartItemDto>.Success(new CartItemDto { Id = cartItem.Id }, "Item added to cart.");
    }

    public async Task<ServiceResponse<bool>> UpdateCartItemQuantityAsync(int cartItemId, int quantity)
    {
        if (quantity <= 0) return await RemoveFromCartAsync(cartItemId);
        
        await _repository.UpdateQuantityAsync(cartItemId, quantity);
        return ServiceResponse<bool>.Success(true, "Quantity updated.");
    }

    public async Task<ServiceResponse<bool>> RemoveFromCartAsync(int cartItemId)
    {
        await _repository.RemoveItemAsync(cartItemId);
        return ServiceResponse<bool>.Success(true, "Item removed from cart.");
    }

    public async Task<ServiceResponse<bool>> ClearUserCartAsync(int userId)
    {
        await _repository.ClearCartAsync(userId);
        return ServiceResponse<bool>.Success(true, "Cart cleared.");
    }
}
