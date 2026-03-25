using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entities;
using Npgsql;

namespace JewelryStore.Api.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    public AuthRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<UserEntity?> GetByEmailAsync(string email)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM users WHERE email = @Email";
        return await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Email = email });
    }

    public async Task<UserEntity?> GetByUsernameAsync(string username)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM users WHERE username = @Username";
        return await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Username = username });
    }

    public async Task<int> CreateAsync(UserEntity user)
    {
        using var connection = CreateConnection();
        const string sql = @"INSERT INTO users (username, email, password_hash, role, created_at) 
                            VALUES (@Username, @Email, @PasswordHash, @Role, @CreatedAt) 
                            RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE email = @Email)";
        return await connection.QuerySingleAsync<bool>(sql, new { Email = email });
    }
}
