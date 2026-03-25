using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entity;
using Npgsql;

namespace JewelryStore.Api.Repositories;

public class JewelryRepository : IJewelryRepository
{
    private readonly string _connectionString;

    public JewelryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<IEnumerable<JewelryItemEntity>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM jewelry_items";
        return await connection.QueryAsync<JewelryItemEntity>(sql);
    }

    public async Task<JewelryItemEntity?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM jewelry_items WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<JewelryItemEntity>(sql, new { Id = id });
    }

    public async Task AddAsync(JewelryItemEntity item)
    {
        using var connection = CreateConnection();
        const string sql = @"INSERT INTO jewelry_items (name, description, price, image_url, category_id) 
                            VALUES (@Name, @Description, @Price, @ImageUrl, @CategoryId) 
                            RETURNING id";
        item.Id = await connection.ExecuteScalarAsync<int>(sql, item);
    }

    public async Task UpdateAsync(JewelryItemEntity item)
    {
        using var connection = CreateConnection();
        const string sql = @"UPDATE jewelry_items 
                            SET name = @Name, description = @Description, price = @Price, 
                                image_url = @ImageUrl, category_id = @CategoryId 
                            WHERE id = @Id";
        await connection.ExecuteAsync(sql, item);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM jewelry_items WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
