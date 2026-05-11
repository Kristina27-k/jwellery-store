using JewelryStore.Api.Models.Entities;

namespace JewelryStore.Api.Repositories;

public interface IPaymentRepository
{
    Task<int> CreateAsync(PaymentTransactionEntity paymentTransaction);
    Task<PaymentTransactionEntity?> GetByOrderIdAsync(string provider, string orderId);
    Task<PaymentTransactionEntity?> GetByProviderSessionIdAsync(string provider, string providerSessionId);
    Task UpdateAsync(PaymentTransactionEntity paymentTransaction);
    Task<IEnumerable<PaymentTransactionEntity>> GetAllAsync();
    Task<IEnumerable<PaymentTransactionEntity>> GetByUserIdAsync(int userId);
}
