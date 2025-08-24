using BlogApi.Application.DTOs;

namespace BlogApi.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponseDto?> LoginAsync(string username, string password);
        Task<AuthenticationResponseDto?> RegisterAsync(RegisterRequestDto request);
        string GenerateJwtToken(UserDto user);
        bool VerifyPassword(string password, string passwordHash);
        string HashPassword(string password);
    }
}