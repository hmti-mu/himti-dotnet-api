using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Models.Requests;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Auth
{
    public class RegisterEndpoint : Endpoint<RegisterRequest, AuthenticationResponseDto>
    {
        public RegisterUserUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/auth/register");
            AllowAnonymous();
            Tags("Authentication");
            Summary(s =>
            {
                s.Summary = "Register a new user";
                s.Description = "Creates a new user account with the provided details";
                s.Response<AuthenticationResponseDto>(201, "User registered successfully");
                s.Response(400, "Bad request - validation failed or user already exists");
            });
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            try
            {
                var registerDto = new RegisterRequestDto
                {
                    Username = req.Username,
                    Email = req.Email,
                    Password = req.Password,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };

                var result = await UseCase.ExecuteAsync(registerDto);
                
                if (result == null)
                {
                    HttpContext.Response.StatusCode = 400;
                    return;
                }

                Response = result;
                HttpContext.Response.StatusCode = 201;
            }
            catch (InvalidOperationException ex)
            {
                HttpContext.Response.StatusCode = 400;
                Response = new AuthenticationResponseDto { Token = "", User = null!, ExpiresAt = DateTime.MinValue };
                AddError(ex.Message);
            }
        }
    }
}