namespace JewelryStore.Api.Models.Settings;

public class PaymentSettings
{
    public string FrontendBaseUrl { get; set; } = "http://localhost:5173";
    public EsewaSettings Esewa { get; set; } = new();
    public KhaltiSettings Khalti { get; set; } = new();
}

public class EsewaSettings
{
    public string ProductCode { get; set; } = "EPAYTEST";
    public string SecretKey { get; set; } = "8gBm/:&EnhH.1/q(";
    public string FormUrl { get; set; } = "https://rc-epay.esewa.com.np/api/epay/main/v2/form";
    public string StatusCheckUrl { get; set; } = "https://rc.esewa.com.np/api/epay/transaction/status/";
}

public class KhaltiSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://dev.khalti.com/api/v2";
}
