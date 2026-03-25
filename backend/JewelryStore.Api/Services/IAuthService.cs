using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;

namespace JewelryStore.Api.Services;

public interface IAuthService
{
    Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest request);
    Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest request);
}
