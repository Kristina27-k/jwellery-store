using System.Security.Claims;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JewelryStore.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return 0;
        return int.Parse(userIdClaim.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();
        var response = await _cartService.GetUserCartAsync(userId);
        return Ok(response);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var userId = GetUserId();
        var response = await _cartService.AddToCartAsync(userId, request.JewelryItemId, request.Quantity);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartRequest request)
    {
        var response = await _cartService.UpdateCartItemQuantityAsync(request.CartItemId, request.Quantity);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var response = await _cartService.RemoveFromCartAsync(id);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();
        var response = await _cartService.ClearUserCartAsync(userId);
        return Ok(response);
    }
}

public class AddToCartRequest
{
    public int JewelryItemId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class UpdateCartRequest
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}
