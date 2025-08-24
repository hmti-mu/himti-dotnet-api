using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;
using BlogApi.Web.Authorization;
using FastEndpoints;
using System.Security.Claims;

namespace BlogApi.Web.Endpoints.Auth
{
    public class GetProfileEndpoint : EndpointWithoutRequest<UserDto>
    {
        public IUserRepository UserRepository { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/auth/profile");
            Policies(PolicyNames.RequireUser);
            Tags("2. Authentication");
            Summary(s =>
            {
                s.Summary = "Get current user profile";
                s.Description = "Retrieves the profile information of the currently authenticated user";
                s.Response<UserDto>(200, "Profile retrieved successfully");
                s.Response(401, "Unauthorized - user not authenticated");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                HttpContext.Response.StatusCode = 401;
                return;
            }

            var user = await UserRepository.GetByIdAsync(userId);
            
            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Response = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                Roles = user.UserRoles.Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    Description = ur.Role.Description,
                    Level = ur.Role.Level
                }).ToList()
            };
        }
    }
}
