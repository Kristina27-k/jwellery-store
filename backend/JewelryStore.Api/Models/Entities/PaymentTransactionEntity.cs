namespace JewelryStore.Api.Models.Entities;

public class PaymentTransactionEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string? ProviderSessionId { get; set; }
    public string? ProviderTransactionId { get; set; }
    public long AmountPaisa { get; set; }
    public decimal AmountRupees { get; set; }
    public string Status { get; set; } = "Initiated";
    public string ClientReturnBaseUrl { get; set; } = string.Empty;
    public string? RawResponse { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
