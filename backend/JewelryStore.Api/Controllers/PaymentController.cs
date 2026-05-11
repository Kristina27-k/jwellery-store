using System.Security.Claims;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Settings;
using JewelryStore.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace JewelryStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly PaymentSettings _paymentSettings;

    public PaymentController(IPaymentService paymentService, IOptions<PaymentSettings> paymentSettings)
    {
        _paymentService = paymentService;
        _paymentSettings = paymentSettings.Value;
    }

    [Authorize]
    [HttpPost("esewa/initiate")]
    public async Task<IActionResult> InitiateEsewa()
    {
        var userId = GetUserId();
        var successUrl = Url.ActionLink(
            nameof(EsewaCallback),
            "Payment",
            values: null,
            protocol: Request.Scheme)
            ?? throw new InvalidOperationException("Unable to build eSewa success callback URL.");

        var failureCallbackBaseUrl = Url.ActionLink(
            nameof(EsewaFailure),
            "Payment",
            values: null,
            protocol: Request.Scheme)
            ?? throw new InvalidOperationException("Unable to build eSewa failure callback URL.");

        var response = await _paymentService.InitiateEsewaAsync(
            userId,
            successUrl,
            failureCallbackBaseUrl,
            ResolveClientReturnBaseUrl());

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        if (response.Data != null)
        {
            response.Data.Fields["success_url"] = QueryHelpers.AddQueryString(successUrl, "orderId", response.Data.OrderId);
            response.Data.Fields["failure_url"] = QueryHelpers.AddQueryString(failureCallbackBaseUrl, "orderId", response.Data.OrderId);
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPost("khalti/initiate")]
    public async Task<IActionResult> InitiateKhalti()
    {
        var userId = GetUserId();
        var clientReturnBaseUrl = ResolveClientReturnBaseUrl();
        var callbackUrl = Url.ActionLink(
            nameof(KhaltiCallback),
            "Payment",
            values: null,
            protocol: Request.Scheme)
            ?? throw new InvalidOperationException("Unable to build Khalti callback URL.");

        var response = await _paymentService.InitiateKhaltiAsync(
            userId,
            callbackUrl,
            clientReturnBaseUrl,
            clientReturnBaseUrl);

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [AllowAnonymous]
    [Route("esewa/callback")]
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> EsewaCallback([FromQuery] string? data, [FromQuery] string? orderId)
    {
        if (string.IsNullOrWhiteSpace(data) && Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync();
            data = form["data"].ToString();
            orderId ??= form["orderId"].ToString();
        }

        var result = await _paymentService.VerifyEsewaAsync(data, orderId);
        return Redirect(BuildFrontendStatusUrl(result));
    }

    [AllowAnonymous]
    [Route("esewa/failure")]
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> EsewaFailure([FromQuery] string? orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId) && Request.HasFormContentType)
        {
            var form = await Request.ReadFormAsync();
            orderId = form["orderId"].ToString();
        }

        var result = await _paymentService.HandleEsewaFailureAsync(orderId);
        return Redirect(BuildFrontendStatusUrl(result));
    }

    [AllowAnonymous]
    [HttpGet("khalti/callback")]
    public async Task<IActionResult> KhaltiCallback(
        [FromQuery] string? pidx,
        [FromQuery] string? purchase_order_id,
        [FromQuery] string? status,
        [FromQuery] string? transaction_id)
    {
        var result = await _paymentService.VerifyKhaltiAsync(pidx, purchase_order_id, status, transaction_id);
        return Redirect(BuildFrontendStatusUrl(result));
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return 0;
        }

        return int.Parse(userIdClaim.Value);
    }

    private string ResolveClientReturnBaseUrl()
    {
        var origin = Request.Headers.Origin.FirstOrDefault();
        if (Uri.TryCreate(origin, UriKind.Absolute, out var originUri))
        {
            return originUri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        }

        var referer = Request.Headers.Referer.FirstOrDefault();
        if (Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
        {
            return refererUri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        }

        return string.IsNullOrWhiteSpace(_paymentSettings.FrontendBaseUrl)
            ? "http://localhost:5173"
            : _paymentSettings.FrontendBaseUrl.TrimEnd('/');
    }

    private static string BuildFrontendStatusUrl(PaymentCallbackResultDto result)
    {
        var query = new Dictionary<string, string?>
        {
            ["provider"] = result.Provider,
            ["status"] = result.Status,
            ["providerStatus"] = result.ProviderStatus,
            ["message"] = result.Message,
            ["orderId"] = result.OrderId,
            ["reference"] = result.ProviderReference
        };

        return QueryHelpers.AddQueryString($"{result.RedirectBaseUrl.TrimEnd('/')}/payment-status", query);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/orders")]
    public async Task<ActionResult<IEnumerable<PaymentTransactionDto>>> GetAllOrders()
    {
        var response = await _paymentService.GetAllTransactionsAsync();
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("user/orders")]
    public async Task<ActionResult<IEnumerable<PaymentTransactionDto>>> GetUserOrders()
    {
        var userId = GetUserId();
        var response = await _paymentService.GetUserTransactionsAsync(userId);
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }
}

