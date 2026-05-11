using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entities;
using Npgsql;

namespace JewelryStore.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly string _connectionString;

    public CategoryRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT id, cat_name, description FROM categories ORDER BY id";
        return await connection.QueryAsync<CategoryEntity>(sql);
    }

    public async Task<CategoryEntity?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT id, cat_name, description FROM categories WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<CategoryEntity>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(CategoryEntity category)
    {
        using var connection = CreateConnection();
        const string sql = @"
            INSERT INTO categories (cat_name, description)
            VALUES (@cat_name, @description)
            RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, new { cat_name = category.cat_name, description = category.Description });
    }

    public async Task UpdateAsync(CategoryEntity category)
    {
        using var connection = CreateConnection();
        const string sql = @"
            UPDATE categories
            SET cat_name = @cat_name, description = @description
            WHERE id = @id";
        await connection.ExecuteAsync(sql, new { id = category.Id, cat_name = category.cat_name, description = category.Description });
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        const string sql = "DELETE FROM categories WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<CategoryEntity?> GetAllCatagoryAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM categories LIMIT 1";
        return await connection.QueryFirstOrDefaultAsync<CategoryEntity>(sql);
    }
}
