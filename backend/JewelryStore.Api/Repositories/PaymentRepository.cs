using System.Data;
using Dapper;
using JewelryStore.Api.Models.Entities;
using Npgsql;

namespace JewelryStore.Api.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly string _connectionString;

    public PaymentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    public async Task<int> CreateAsync(PaymentTransactionEntity paymentTransaction)
    {
        using var connection = CreateConnection();
        const string sql = @"
            INSERT INTO payment_transactions
            (
                user_id,
                provider,
                order_id,
                provider_session_id,
                provider_transaction_id,
                amount_paisa,
                amount_rupees,
                status,
                client_return_base_url,
                raw_response,
                created_at,
                updated_at,
                completed_at
            )
            VALUES
            (
                @UserId,
                @Provider,
                @OrderId,
                @ProviderSessionId,
                @ProviderTransactionId,
                @AmountPaisa,
                @AmountRupees,
                @Status,
                @ClientReturnBaseUrl,
                @RawResponse,
                @CreatedAt,
                @UpdatedAt,
                @CompletedAt
            )
            RETURNING id";

        return await connection.ExecuteScalarAsync<int>(sql, paymentTransaction);
    }

    public async Task<PaymentTransactionEntity?> GetByOrderIdAsync(string provider, string orderId)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT *
            FROM payment_transactions
            WHERE provider = @Provider AND order_id = @OrderId";

        return await connection.QueryFirstOrDefaultAsync<PaymentTransactionEntity>(sql, new { Provider = provider, OrderId = orderId });
    }

    public async Task<PaymentTransactionEntity?> GetByProviderSessionIdAsync(string provider, string providerSessionId)
    {
        using var connection = CreateConnection();
        const string sql = @"
            SELECT *
            FROM payment_transactions
            WHERE provider = @Provider AND provider_session_id = @ProviderSessionId";

        return await connection.QueryFirstOrDefaultAsync<PaymentTransactionEntity>(sql, new { Provider = provider, ProviderSessionId = providerSessionId });
    }

    public async Task UpdateAsync(PaymentTransactionEntity paymentTransaction)
    {
        using var connection = CreateConnection();
        const string sql = @"
            UPDATE payment_transactions
            SET
                provider_session_id = @ProviderSessionId,
                provider_transaction_id = @ProviderTransactionId,
                amount_paisa = @AmountPaisa,
                amount_rupees = @AmountRupees,
                status = @Status,
                client_return_base_url = @ClientReturnBaseUrl,
                raw_response = @RawResponse,
                updated_at = @UpdatedAt,
                completed_at = @CompletedAt
            WHERE id = @Id";

        await connection.ExecuteAsync(sql, paymentTransaction);
    }

    public async Task<IEnumerable<PaymentTransactionEntity>> GetAllAsync()
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM payment_transactions ORDER BY created_at DESC";
        return await connection.QueryAsync<PaymentTransactionEntity>(sql);
    }

    public async Task<IEnumerable<PaymentTransactionEntity>> GetByUserIdAsync(int userId)
    {
        using var connection = CreateConnection();
        const string sql = "SELECT * FROM payment_transactions WHERE user_id = @UserId ORDER BY created_at DESC";
        return await connection.QueryAsync<PaymentTransactionEntity>(sql, new { UserId = userId });
    }
}

