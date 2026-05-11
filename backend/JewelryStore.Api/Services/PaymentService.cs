using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Entities;
using JewelryStore.Api.Models.Settings;
using JewelryStore.Api.Repositories;
using Microsoft.Extensions.Options;

namespace JewelryStore.Api.Services;

public class PaymentService : IPaymentService
{
    private const string EsewaProvider = "esewa";
    private const string KhaltiProvider = "khalti";
    private readonly ICartService _cartService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly PaymentSettings _paymentSettings;

    public PaymentService(
        ICartService cartService,
        IPaymentRepository paymentRepository,
        IHttpClientFactory httpClientFactory,
        IOptions<PaymentSettings> paymentSettings)
    {
        _cartService = cartService;
        _paymentRepository = paymentRepository;
        _httpClientFactory = httpClientFactory;
        _paymentSettings = paymentSettings.Value;
    }

    public async Task<ServiceResponse<EsewaInitiateResponseDto>> InitiateEsewaAsync(
        int userId,
        string successUrl,
        string failureUrl,
        string clientReturnBaseUrl)
    {
        var cartItemsResponse = await _cartService.GetUserCartAsync(userId);
        if (!cartItemsResponse.IsSuccess || cartItemsResponse.Data == null)
        {
            return ServiceResponse<EsewaInitiateResponseDto>.Failure(cartItemsResponse.Message);
        }

        var cartItems = cartItemsResponse.Data.ToList();
        if (cartItems.Count == 0)
        {
            return ServiceResponse<EsewaInitiateResponseDto>.Failure("Your cart is empty.");
        }

        var totalAmount = cartItems.Sum(item => item.TotalPrice);
        var orderId = GenerateOrderId("ESW", userId);
        var totalAmountText = FormatEsewaAmount(totalAmount);
        var signatureMessage =
            $"total_amount={totalAmountText},transaction_uuid={orderId},product_code={_paymentSettings.Esewa.ProductCode}";
        var signature = GenerateHmacSha256Base64(signatureMessage, _paymentSettings.Esewa.SecretKey);

        var transaction = new PaymentTransactionEntity
        {
            UserId = userId,
            Provider = EsewaProvider,
            OrderId = orderId,
            AmountPaisa = ConvertToPaisa(totalAmount),
            AmountRupees = totalAmount,
            Status = "Initiated",
            ClientReturnBaseUrl = NormalizeBaseUrl(clientReturnBaseUrl),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        transaction.Id = await _paymentRepository.CreateAsync(transaction);

        var payload = new EsewaInitiateResponseDto
        {
            OrderId = orderId,
            ActionUrl = _paymentSettings.Esewa.FormUrl,
            Fields = new Dictionary<string, string>
            {
                ["amount"] = totalAmountText,
                ["tax_amount"] = "0",
                ["total_amount"] = totalAmountText,
                ["transaction_uuid"] = orderId,
                ["product_code"] = _paymentSettings.Esewa.ProductCode,
                ["product_service_charge"] = "0",
                ["product_delivery_charge"] = "0",
                ["success_url"] = successUrl,
                ["failure_url"] = failureUrl,
                ["signed_field_names"] = "total_amount,transaction_uuid,product_code",
                ["signature"] = signature
            }
        };

        return ServiceResponse<EsewaInitiateResponseDto>.Success(payload, "eSewa payment initialized.");
    }

    public async Task<ServiceResponse<KhaltiInitiateResponseDto>> InitiateKhaltiAsync(
        int userId,
        string returnUrl,
        string websiteUrl,
        string clientReturnBaseUrl)
    {
        if (string.IsNullOrWhiteSpace(_paymentSettings.Khalti.SecretKey))
        {
            return ServiceResponse<KhaltiInitiateResponseDto>.Failure(
                "Khalti secret key is missing. Add Payment:Khalti:SecretKey to backend configuration.");
        }

        var cartItemsResponse = await _cartService.GetUserCartAsync(userId);
        if (!cartItemsResponse.IsSuccess || cartItemsResponse.Data == null)
        {
            return ServiceResponse<KhaltiInitiateResponseDto>.Failure(cartItemsResponse.Message);
        }

        var cartItems = cartItemsResponse.Data.ToList();
        if (cartItems.Count == 0)
        {
            return ServiceResponse<KhaltiInitiateResponseDto>.Failure("Your cart is empty.");
        }

        var totalAmount = cartItems.Sum(item => item.TotalPrice);
        var amountPaisa = ConvertToPaisa(totalAmount);
        var orderId = GenerateOrderId("KLT", userId);
        var purchaseOrderName = cartItems.Count == 1 ? cartItems[0].ProductName : $"{cartItems.Count} jewelry items";

        var transaction = new PaymentTransactionEntity
        {
            UserId = userId,
            Provider = KhaltiProvider,
            OrderId = orderId,
            AmountPaisa = amountPaisa,
            AmountRupees = totalAmount,
            Status = "Initiated",
            ClientReturnBaseUrl = NormalizeBaseUrl(clientReturnBaseUrl),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        transaction.Id = await _paymentRepository.CreateAsync(transaction);

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {_paymentSettings.Khalti.SecretKey}");

        var requestPayload = new
        {
            return_url = returnUrl,
            website_url = websiteUrl,
            amount = amountPaisa,
            purchase_order_id = orderId,
            purchase_order_name = purchaseOrderName,
            product_details = cartItems.Select(item => new
            {
                identity = item.JewelryItemId.ToString(CultureInfo.InvariantCulture),
                name = item.ProductName,
                total_price = ConvertToPaisa(item.TotalPrice),
                quantity = item.Quantity,
                unit_price = ConvertToPaisa(item.Price)
            })
        };

        using var response = await httpClient.PostAsJsonAsync(
            $"{_paymentSettings.Khalti.BaseUrl.TrimEnd('/')}/epayment/initiate/",
            requestPayload);

        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            transaction.Status = "Failed";
            transaction.RawResponse = responseContent;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return ServiceResponse<KhaltiInitiateResponseDto>.Failure(ExtractErrorMessage(responseContent) ?? "Khalti initiation failed.");
        }

        var khaltiResponse = JsonSerializer.Deserialize<KhaltiInitiateApiResponse>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (khaltiResponse == null || string.IsNullOrWhiteSpace(khaltiResponse.Pidx) || string.IsNullOrWhiteSpace(khaltiResponse.PaymentUrl))
        {
            transaction.Status = "Failed";
            transaction.RawResponse = responseContent;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return ServiceResponse<KhaltiInitiateResponseDto>.Failure("Khalti returned an invalid initiation response.");
        }

        transaction.ProviderSessionId = khaltiResponse.Pidx;
        transaction.RawResponse = responseContent;
        transaction.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(transaction);

        return ServiceResponse<KhaltiInitiateResponseDto>.Success(
            new KhaltiInitiateResponseDto
            {
                OrderId = orderId,
                Pidx = khaltiResponse.Pidx,
                PaymentUrl = khaltiResponse.PaymentUrl,
                ExpiresAt = khaltiResponse.ExpiresAt,
                ExpiresIn = khaltiResponse.ExpiresIn
            },
            "Khalti payment initialized.");
    }

    public async Task<PaymentCallbackResultDto> VerifyEsewaAsync(string? encodedPayload, string? orderId)
    {
        if (string.IsNullOrWhiteSpace(encodedPayload))
        {
            return await BuildFailedCallbackResultAsync(EsewaProvider, orderId, "Missing eSewa callback data.");
        }

        var decodedPayload = TryDecodeBase64Payload(encodedPayload);
        if (string.IsNullOrWhiteSpace(decodedPayload))
        {
            return await BuildFailedCallbackResultAsync(EsewaProvider, orderId, "Unable to decode eSewa callback payload.");
        }

        Dictionary<string, string> callbackFields;
        try
        {
            callbackFields = ParseJsonFields(decodedPayload);
        }
        catch
        {
            return await BuildFailedCallbackResultAsync(EsewaProvider, orderId, "Invalid eSewa callback payload.");
        }

        if (!callbackFields.TryGetValue("transaction_uuid", out var transactionUuid) || string.IsNullOrWhiteSpace(transactionUuid))
        {
            transactionUuid = orderId ?? string.Empty;
        }

        var transaction = await _paymentRepository.GetByOrderIdAsync(EsewaProvider, transactionUuid);
        if (transaction == null)
        {
            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transactionUuid,
                uiStatus: "failed",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "The eSewa transaction could not be matched to a cart.",
                redirectBaseUrl: GetFallbackReturnBaseUrl(),
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        if (!callbackFields.TryGetValue("signature", out var signature) || string.IsNullOrWhiteSpace(signature))
        {
            transaction.Status = "Failed";
            transaction.RawResponse = decodedPayload;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "eSewa callback signature is missing.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        var signedMessage = BuildSignedFieldMessage(callbackFields);
        if (string.IsNullOrWhiteSpace(signedMessage))
        {
            transaction.Status = "Failed";
            transaction.RawResponse = decodedPayload;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "eSewa callback fields were incomplete.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        var expectedSignature = GenerateHmacSha256Base64(signedMessage, _paymentSettings.Esewa.SecretKey);
        if (!FixedTimeEquals(signature, expectedSignature))
        {
            transaction.Status = "Failed";
            transaction.RawResponse = decodedPayload;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "eSewa callback signature verification failed.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        if (!string.Equals(
                callbackFields.GetValueOrDefault("product_code"),
                _paymentSettings.Esewa.ProductCode,
                StringComparison.Ordinal))
        {
            transaction.Status = "Failed";
            transaction.RawResponse = decodedPayload;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "eSewa product code did not match the configured merchant code.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        var lookupResponse = await LookupEsewaPaymentAsync(transaction.OrderId, transaction.AmountRupees);
        if (lookupResponse == null)
        {
            transaction.Status = "Pending";
            transaction.RawResponse = decodedPayload;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "pending",
                providerStatus: callbackFields.GetValueOrDefault("status") ?? "UNKNOWN",
                message: "eSewa verification is pending. Check the transaction again shortly.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: callbackFields.GetValueOrDefault("transaction_code"));
        }

        transaction.ProviderTransactionId = callbackFields.GetValueOrDefault("transaction_code") ?? lookupResponse.RefId;
        transaction.RawResponse = JsonSerializer.Serialize(new { callback = callbackFields, lookup = lookupResponse });
        transaction.UpdatedAt = DateTime.UtcNow;

        var providerStatus = lookupResponse.Status ?? callbackFields.GetValueOrDefault("status") ?? "UNKNOWN";
        if (string.Equals(providerStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase))
        {
            if (lookupResponse.TotalAmount != transaction.AmountRupees)
            {
                transaction.Status = "Failed";
                await _paymentRepository.UpdateAsync(transaction);

                return BuildCallbackResult(
                    provider: EsewaProvider,
                    orderId: transaction.OrderId,
                    uiStatus: "failed",
                    providerStatus: providerStatus,
                    message: "eSewa verification returned an amount that did not match the cart total.",
                    redirectBaseUrl: transaction.ClientReturnBaseUrl,
                    providerReference: transaction.ProviderTransactionId);
            }

            transaction.Status = "Completed";
            transaction.CompletedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);
            await _cartService.ClearUserCartAsync(transaction.UserId);

            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: transaction.OrderId,
                uiStatus: "success",
                providerStatus: providerStatus,
                message: "eSewa payment completed successfully.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transaction.ProviderTransactionId);
        }

        transaction.Status = NormalizeProviderStatus(providerStatus);
        await _paymentRepository.UpdateAsync(transaction);

        return BuildCallbackResult(
            provider: EsewaProvider,
            orderId: transaction.OrderId,
            uiStatus: MapToUiStatus(providerStatus),
            providerStatus: providerStatus,
            message: BuildProviderMessage(EsewaProvider, providerStatus),
            redirectBaseUrl: transaction.ClientReturnBaseUrl,
            providerReference: transaction.ProviderTransactionId);
    }

    public async Task<PaymentCallbackResultDto> HandleEsewaFailureAsync(string? orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
        {
            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: string.Empty,
                uiStatus: "failed",
                providerStatus: "FAILED",
                message: "eSewa payment was canceled or did not complete.",
                redirectBaseUrl: GetFallbackReturnBaseUrl(),
                providerReference: null);
        }

        var transaction = await _paymentRepository.GetByOrderIdAsync(EsewaProvider, orderId);
        if (transaction == null)
        {
            return BuildCallbackResult(
                provider: EsewaProvider,
                orderId: orderId,
                uiStatus: "failed",
                providerStatus: "FAILED",
                message: "eSewa payment was canceled or did not complete.",
                redirectBaseUrl: GetFallbackReturnBaseUrl(),
                providerReference: null);
        }

        transaction.Status = "Failed";
        transaction.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.UpdateAsync(transaction);

        return BuildCallbackResult(
            provider: EsewaProvider,
            orderId: transaction.OrderId,
            uiStatus: "failed",
            providerStatus: "FAILED",
            message: "eSewa payment was canceled or is still pending.",
            redirectBaseUrl: transaction.ClientReturnBaseUrl,
            providerReference: null);
    }

    public async Task<PaymentCallbackResultDto> VerifyKhaltiAsync(
        string? pidx,
        string? purchaseOrderId,
        string? callbackStatus,
        string? transactionId)
    {
        if (string.IsNullOrWhiteSpace(_paymentSettings.Khalti.SecretKey))
        {
            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: purchaseOrderId ?? string.Empty,
                uiStatus: "failed",
                providerStatus: callbackStatus ?? "UNKNOWN",
                message: "Khalti secret key is missing on the backend.",
                redirectBaseUrl: GetFallbackReturnBaseUrl(),
                providerReference: transactionId);
        }

        PaymentTransactionEntity? transaction = null;
        if (!string.IsNullOrWhiteSpace(purchaseOrderId))
        {
            transaction = await _paymentRepository.GetByOrderIdAsync(KhaltiProvider, purchaseOrderId);
        }

        if (transaction == null && !string.IsNullOrWhiteSpace(pidx))
        {
            transaction = await _paymentRepository.GetByProviderSessionIdAsync(KhaltiProvider, pidx);
        }

        if (transaction == null)
        {
            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: purchaseOrderId ?? string.Empty,
                uiStatus: "failed",
                providerStatus: callbackStatus ?? "UNKNOWN",
                message: "The Khalti transaction could not be matched to a cart.",
                redirectBaseUrl: GetFallbackReturnBaseUrl(),
                providerReference: transactionId);
        }

        if (!string.IsNullOrWhiteSpace(purchaseOrderId) &&
            !string.Equals(transaction.OrderId, purchaseOrderId, StringComparison.Ordinal))
        {
            transaction.Status = "Failed";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackStatus ?? "UNKNOWN",
                message: "Khalti callback order ID did not match the initiated transaction.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transactionId);
        }

        if (string.IsNullOrWhiteSpace(pidx))
        {
            transaction.Status = "Failed";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: callbackStatus ?? "UNKNOWN",
                message: "Khalti did not return a payment identifier.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transactionId);
        }

        var lookupResponse = await LookupKhaltiPaymentAsync(pidx);
        if (lookupResponse == null)
        {
            transaction.Status = "Pending";
            transaction.ProviderSessionId = pidx;
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: transaction.OrderId,
                uiStatus: "pending",
                providerStatus: callbackStatus ?? "UNKNOWN",
                message: "Khalti verification is pending. Check the payment again shortly.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transactionId);
        }

        transaction.ProviderSessionId = lookupResponse.Pidx ?? pidx;
        transaction.ProviderTransactionId = lookupResponse.TransactionId ?? transactionId;
        transaction.RawResponse = JsonSerializer.Serialize(lookupResponse);
        transaction.UpdatedAt = DateTime.UtcNow;

        var providerStatus = lookupResponse.Status ?? callbackStatus ?? "UNKNOWN";
        if (string.Equals(providerStatus, "Completed", StringComparison.OrdinalIgnoreCase) &&
            lookupResponse.TotalAmount == transaction.AmountPaisa)
        {
            transaction.Status = "Completed";
            transaction.CompletedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);
            await _cartService.ClearUserCartAsync(transaction.UserId);

            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: transaction.OrderId,
                uiStatus: "success",
                providerStatus: providerStatus,
                message: "Khalti payment completed successfully.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transaction.ProviderTransactionId);
        }

        if (string.Equals(providerStatus, "Completed", StringComparison.OrdinalIgnoreCase) &&
            lookupResponse.TotalAmount != transaction.AmountPaisa)
        {
            transaction.Status = "Failed";
            await _paymentRepository.UpdateAsync(transaction);

            return BuildCallbackResult(
                provider: KhaltiProvider,
                orderId: transaction.OrderId,
                uiStatus: "failed",
                providerStatus: providerStatus,
                message: "Khalti verification returned an amount that did not match the cart total.",
                redirectBaseUrl: transaction.ClientReturnBaseUrl,
                providerReference: transaction.ProviderTransactionId);
        }

        transaction.Status = NormalizeProviderStatus(providerStatus);
        await _paymentRepository.UpdateAsync(transaction);

        return BuildCallbackResult(
            provider: KhaltiProvider,
            orderId: transaction.OrderId,
            uiStatus: MapToUiStatus(providerStatus),
            providerStatus: providerStatus,
            message: BuildProviderMessage(KhaltiProvider, providerStatus),
            redirectBaseUrl: transaction.ClientReturnBaseUrl,
            providerReference: transaction.ProviderTransactionId);
    }

    private async Task<EsewaStatusResponse?> LookupEsewaPaymentAsync(string orderId, decimal amountRupees)
    {
        var query = new Dictionary<string, string?>
        {
            ["product_code"] = _paymentSettings.Esewa.ProductCode,
            ["total_amount"] = FormatEsewaAmount(amountRupees),
            ["transaction_uuid"] = orderId
        };

        var url = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(
            _paymentSettings.Esewa.StatusCheckUrl,
            query);

        var httpClient = _httpClientFactory.CreateClient();
        using var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EsewaStatusResponse>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<KhaltiLookupResponse?> LookupKhaltiPaymentAsync(string pidx)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Key {_paymentSettings.Khalti.SecretKey}");

        using var response = await httpClient.PostAsJsonAsync(
            $"{_paymentSettings.Khalti.BaseUrl.TrimEnd('/')}/epayment/lookup/",
            new { pidx });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<KhaltiLookupResponse>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<PaymentCallbackResultDto> BuildFailedCallbackResultAsync(string provider, string? orderId, string message)
    {
        PaymentTransactionEntity? transaction = null;
        if (!string.IsNullOrWhiteSpace(orderId))
        {
            transaction = await _paymentRepository.GetByOrderIdAsync(provider, orderId);
        }

        if (transaction != null)
        {
            transaction.Status = "Failed";
            transaction.UpdatedAt = DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(transaction);
        }

        return BuildCallbackResult(
            provider: provider,
            orderId: orderId ?? string.Empty,
            uiStatus: "failed",
            providerStatus: "FAILED",
            message: message,
            redirectBaseUrl: transaction?.ClientReturnBaseUrl ?? GetFallbackReturnBaseUrl(),
            providerReference: null);
    }

    public async Task<ServiceResponse<IEnumerable<PaymentTransactionDto>>> GetAllTransactionsAsync()
    {
        var transactions = await _paymentRepository.GetAllAsync();
        var dtos = transactions.Select(MapToPaymentTransactionDto).ToList();
        return ServiceResponse<IEnumerable<PaymentTransactionDto>>.Success(dtos, "Transactions retrieved successfully.");
    }

    public async Task<ServiceResponse<IEnumerable<PaymentTransactionDto>>> GetUserTransactionsAsync(int userId)
    {
        var transactions = await _paymentRepository.GetByUserIdAsync(userId);
        var dtos = transactions.Select(MapToPaymentTransactionDto).ToList();
        return ServiceResponse<IEnumerable<PaymentTransactionDto>>.Success(dtos, "User transactions retrieved successfully.");
    }

    private static PaymentTransactionDto MapToPaymentTransactionDto(PaymentTransactionEntity entity) => new()
    {
        Id = entity.Id,
        UserId = entity.UserId,
        Provider = entity.Provider,
        OrderId = entity.OrderId,
        AmountRupees = entity.AmountRupees,
        Status = entity.Status,
        ProviderTransactionId = entity.ProviderTransactionId,
        CreatedAt = entity.CreatedAt,
        CompletedAt = entity.CompletedAt
    };

    private static Dictionary<string, string> ParseJsonFields(string json)
    {
        using var document = JsonDocument.Parse(json);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in document.RootElement.EnumerateObject())
        {
            result[property.Name] = property.Value.ValueKind switch
            {
                JsonValueKind.String => property.Value.GetString() ?? string.Empty,
                JsonValueKind.Number => property.Value.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => property.Value.GetRawText()
            };
        }

        return result;
    }

    private static string BuildSignedFieldMessage(IReadOnlyDictionary<string, string> fields)
    {
        if (!fields.TryGetValue("signed_field_names", out var signedFieldNames) || string.IsNullOrWhiteSpace(signedFieldNames))
        {
            return string.Empty;
        }

        var pairs = new List<string>();
        foreach (var fieldName in signedFieldNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!fields.TryGetValue(fieldName, out var value))
            {
                return string.Empty;
            }

            pairs.Add($"{fieldName}={value}");
        }

        return string.Join(",", pairs);
    }

    private static string GenerateHmacSha256Base64(string message, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(messageBytes);
        return Convert.ToBase64String(hash);
    }

    private static bool FixedTimeEquals(string actual, string expected)
    {
        var actualBytes = Encoding.UTF8.GetBytes(actual);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        return actualBytes.Length == expectedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(actualBytes, expectedBytes);
    }

    private static string? TryDecodeBase64Payload(string payload)
    {
        try
        {
            var normalized = Uri.UnescapeDataString(payload).Replace(" ", "+", StringComparison.Ordinal);
            var bytes = Convert.FromBase64String(normalized);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return null;
        }
    }

    private static string GenerateOrderId(string prefix, int userId) =>
        $"{prefix}-U{userId}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

    private static long ConvertToPaisa(decimal amount) =>
        (long)Math.Round(amount * 100m, MidpointRounding.AwayFromZero);

    private static string FormatEsewaAmount(decimal amount) =>
        amount % 1 == 0
            ? amount.ToString("0", CultureInfo.InvariantCulture)
            : amount.ToString("0.##", CultureInfo.InvariantCulture);

    private string NormalizeBaseUrl(string? baseUrl)
    {
        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            return baseUrl.TrimEnd('/');
        }

        return GetFallbackReturnBaseUrl();
    }

    private string GetFallbackReturnBaseUrl() =>
        string.IsNullOrWhiteSpace(_paymentSettings.FrontendBaseUrl)
            ? "http://localhost:5173"
            : _paymentSettings.FrontendBaseUrl.TrimEnd('/');

    private static string NormalizeProviderStatus(string providerStatus)
    {
        if (string.Equals(providerStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            return "Completed";
        }

        if (string.Equals(providerStatus, "PENDING", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Pending", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "AMBIGUOUS", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Initiated", StringComparison.OrdinalIgnoreCase))
        {
            return "Pending";
        }

        return "Failed";
    }

    private static string MapToUiStatus(string providerStatus)
    {
        if (string.Equals(providerStatus, "COMPLETE", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Completed", StringComparison.OrdinalIgnoreCase))
        {
            return "success";
        }

        if (string.Equals(providerStatus, "PENDING", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Pending", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "AMBIGUOUS", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(providerStatus, "Initiated", StringComparison.OrdinalIgnoreCase))
        {
            return "pending";
        }

        return "failed";
    }

    private static string BuildProviderMessage(string provider, string providerStatus)
    {
        var providerName = provider.Equals(KhaltiProvider, StringComparison.OrdinalIgnoreCase) ? "Khalti" : "eSewa";

        return providerStatus.ToUpperInvariant() switch
        {
            "COMPLETE" => $"{providerName} payment completed successfully.",
            "COMPLETED" => $"{providerName} payment completed successfully.",
            "PENDING" => $"{providerName} payment is still pending.",
            "AMBIGUOUS" => $"{providerName} could not confirm the payment yet.",
            "INITIATED" => $"{providerName} payment is still being processed.",
            "CANCELED" => $"{providerName} payment was canceled.",
            "USER CANCELED" => $"{providerName} payment was canceled.",
            "NOT_FOUND" => $"{providerName} could not find the transaction.",
            "FULL_REFUND" => $"{providerName} payment was refunded.",
            "PARTIAL_REFUND" => $"{providerName} payment was partially refunded.",
            _ => $"{providerName} payment could not be completed."
        };
    }

    private static PaymentCallbackResultDto BuildCallbackResult(
        string provider,
        string orderId,
        string uiStatus,
        string providerStatus,
        string message,
        string redirectBaseUrl,
        string? providerReference) =>
        new()
        {
            Provider = provider,
            OrderId = orderId,
            Status = uiStatus,
            ProviderStatus = providerStatus,
            Message = message,
            RedirectBaseUrl = redirectBaseUrl,
            ProviderReference = providerReference
        };

    private static string? ExtractErrorMessage(string content)
    {
        try
        {
            using var document = JsonDocument.Parse(content);
            if (document.RootElement.TryGetProperty("detail", out var detailElement))
            {
                return detailElement.GetString();
            }

            foreach (var property in document.RootElement.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Array && property.Value.GetArrayLength() > 0)
                {
                    return property.Value[0].GetString();
                }

                if (property.Value.ValueKind == JsonValueKind.String)
                {
                    return property.Value.GetString();
                }
            }
        }
        catch
        {
            // Ignore parse errors and fall back to null.
        }

        return null;
    }

    private sealed class KhaltiInitiateApiResponse
    {
        [JsonPropertyName("pidx")]
        public string? Pidx { get; set; }

        [JsonPropertyName("payment_url")]
        public string? PaymentUrl { get; set; }

        [JsonPropertyName("expires_at")]
        public string? ExpiresAt { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }

    private sealed class KhaltiLookupResponse
    {
        [JsonPropertyName("pidx")]
        public string? Pidx { get; set; }

        [JsonPropertyName("total_amount")]
        public long TotalAmount { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("transaction_id")]
        public string? TransactionId { get; set; }
    }

    private sealed class EsewaStatusResponse
    {
        [JsonPropertyName("product_code")]
        public string? ProductCode { get; set; }

        [JsonPropertyName("transaction_uuid")]
        public string? TransactionUuid { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("ref_id")]
        public string? RefId { get; set; }
    }
}
