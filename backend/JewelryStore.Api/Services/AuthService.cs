using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JewelryStore.Api.Models.Common;
using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Models.Entities;
using JewelryStore.Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace JewelryStore.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthRepository authRepository, IConfiguration configuration)
    {
        _authRepository = authRepository;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await _authRepository.ExistsAsync(request.Email))
        {
            return ServiceResponse<AuthResponse>.Failure("User with this email already exists.");
        }

        var user = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BC.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        var userId = await _authRepository.CreateAsync(user);
        user.Id = userId;

        return ServiceResponse<AuthResponse>.Success(new AuthResponse
        {
            Token = GenerateJwtToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        }, "Registration successful.");
    }

    public async Task<ServiceResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _authRepository.GetByEmailAsync(request.Email);
        
        if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !BC.Verify(request.Password, user.PasswordHash))
        {
            return ServiceResponse<AuthResponse>.Failure("Invalid email or password.");
        }

        return ServiceResponse<AuthResponse>.Success(new AuthResponse
        {
            Token = GenerateJwtToken(user),
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        }, "Login successful.");
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
