namespace JewelryStore.Api.Models.Entities;

public class CartItemEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int JewelryItemId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
