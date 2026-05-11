using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface IPaymentService
{
    Task<ServiceResponse<EsewaInitiateResponseDto>> InitiateEsewaAsync(
        int userId,
        string successUrl,
        string failureUrl,
        string clientReturnBaseUrl);

    Task<ServiceResponse<KhaltiInitiateResponseDto>> InitiateKhaltiAsync(
        int userId,
        string returnUrl,
        string websiteUrl,
        string clientReturnBaseUrl);

    Task<PaymentCallbackResultDto> VerifyEsewaAsync(string? encodedPayload, string? orderId);
    Task<PaymentCallbackResultDto> HandleEsewaFailureAsync(string? orderId);
    Task<PaymentCallbackResultDto> VerifyKhaltiAsync(string? pidx, string? purchaseOrderId, string? callbackStatus, string? transactionId);
    Task<ServiceResponse<IEnumerable<PaymentTransactionDto>>> GetAllTransactionsAsync();
    Task<ServiceResponse<IEnumerable<PaymentTransactionDto>>> GetUserTransactionsAsync(int userId);
}
