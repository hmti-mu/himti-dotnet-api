using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Models.Requests;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Auth
{
    public class LoginEndpoint : Endpoint<LoginRequest, AuthenticationResponseDto>
    {
        public LoginUserUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous();
            Tags("Authentication");
            Summary(s =>
            {
                s.Summary = "User login";
                s.Description = "Authenticates a user and returns a JWT token";
                s.Response<AuthenticationResponseDto>(200, "Login successful");
                s.Response(401, "Unauthorized - invalid credentials");
                s.Response(400, "Bad request - validation failed");
            });
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var result = await UseCase.ExecuteAsync(req.Username, req.Password);
            
            if (result == null)
            {
                HttpContext.Response.StatusCode = 401;
                Response = new AuthenticationResponseDto { Token = "", User = null!, ExpiresAt = DateTime.MinValue };
                AddError("Invalid username or password");
                return;
            }

            Response = result;
        }
    }
}