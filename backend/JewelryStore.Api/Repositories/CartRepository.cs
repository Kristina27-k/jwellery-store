using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entities;
using Npgsql;

namespace JewelryStore.Api.Repositories;

public class CartRepository : ICartRepository
{
    private readonly string _connectionString;

    public CartRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<IEnumerable<dynamic>> GetByUserIdAsync(int userId)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT 
                ci.id, 
                ci.jewelry_item_id as JewelryItemId, 
                ji.name as ProductName, 
                ji.price as Price, 
                ji.image_url as ImageUrl, 
                ci.quantity
            FROM cart_items ci
            JOIN jewelry_items ji ON ci.jewelry_item_id = ji.id
            WHERE ci.user_id = @UserId";
        return await connection.QueryAsync(sql, new { UserId = userId });
    }

    public async Task<int> AddItemAsync(CartItemEntity item)
    {
        using var connection = CreateConnection();
        const string sql = @"
            INSERT INTO cart_items (user_id, jewelry_item_id, quantity) 
            VALUES (@UserId, @JewelryItemId, @Quantity) 
            RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, item);
    }

    public async Task UpdateQuantityAsync(int id, int quantity)
    {
        using var connection = CreateConnection();
        const string sql = "UPDATE cart_items SET quantity = @Quantity WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id, Quantity = quantity });
    }

    public async Task RemoveItemAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM cart_items WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task ClearCartAsync(int userId)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM cart_items WHERE user_id = @UserId";
        await connection.ExecuteAsync(sql, new { UserId = userId });
    }

    public async Task<CartItemEntity?> GetByUserIdAndItemIdAsync(int userId, int itemId)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM cart_items WHERE user_id = @UserId AND jewelry_item_id = @ItemId";
        return await connection.QueryFirstOrDefaultAsync<CartItemEntity>(sql, new { UserId = userId, ItemId = itemId });
    }
}
