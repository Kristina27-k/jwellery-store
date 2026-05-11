namespace JewelryStore.Api.Models.DTOs;

public class EsewaInitiateResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public string ActionUrl { get; set; } = string.Empty;
    public Dictionary<string, string> Fields { get; set; } = new();
}

public class KhaltiInitiateResponseDto
{
    public string OrderId { get; set; } = string.Empty;
    public string Pidx { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
    public string? ExpiresAt { get; set; }
    public int ExpiresIn { get; set; }
}

public class PaymentCallbackResultDto
{
    public string Provider { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string Status { get; set; } = "failed";
    public string ProviderStatus { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string RedirectBaseUrl { get; set; } = string.Empty;
    public string? ProviderReference { get; set; }
}

public class PaymentTransactionDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal AmountRupees { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ProviderTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
