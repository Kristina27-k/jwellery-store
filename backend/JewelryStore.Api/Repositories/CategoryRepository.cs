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

    public async Task<CategoryEntity?> GetAllCatagoryAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM categories LIMIT 1";
        return await connection.QueryFirstOrDefaultAsync<CategoryEntity>(sql);
    }
}