using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entities;
using Npgsql;

namespace JewelryStore.Api.Repositories;

/*
public class AuthRepository : IAuthRepository
{
  private readonly string _connectionString;
  public AuthRepository(IConfiguration configuration){
    _connectionString = configuration.GetConnectionString("DefaultConnection");

  }
  private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
  
  // public async Task<IEnumerable<>>
}
*/
