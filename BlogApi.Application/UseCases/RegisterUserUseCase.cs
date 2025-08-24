using BlogApi.Application.DTOs;
using BlogApi.Application.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class RegisterUserUseCase
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterUserUseCase(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AuthenticationResponseDto?> ExecuteAsync(RegisterRequestDto request)
        {
            return await _authenticationService.RegisterAsync(request);
        }
    }
}