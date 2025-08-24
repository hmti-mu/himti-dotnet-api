using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class LoginUserUseCase
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginUserUseCase(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AuthenticationResponseDto?> ExecuteAsync(string username, string password)
        {
            return await _authenticationService.LoginAsync(username, password);
        }
    }
}