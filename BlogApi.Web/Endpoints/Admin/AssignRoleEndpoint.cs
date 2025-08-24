using BlogApi.Domain.Interfaces;
using BlogApi.Web.Authorization;
using BlogApi.Web.Models.Requests;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Admin
{
    public class AssignRoleRequestModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class AssignRoleEndpoint : Endpoint<AssignRoleRequestModel>
    {
        public IRoleRepository RoleRepository { get; set; } = null!;
        public IUserRepository UserRepository { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/admin/users/{userId}/role");
            Policies(PolicyNames.RequireAdmin);
            Tags("3. Administration");
            Summary(s =>
            {
                s.Summary = "Assign role to user";
                s.Description = "Assigns a role to a specific user (Admin only)";
                s.Response(200, "Role assigned successfully");
                s.Response(400, "Bad request - invalid role or user");
                s.Response(401, "Unauthorized - user not authenticated");
                s.Response(403, "Forbidden - insufficient permissions");
                s.Response(404, "User or role not found");
            });
        }

        public override async Task HandleAsync(AssignRoleRequestModel req, CancellationToken ct)
        {
            // Verify user exists
            if (!await UserRepository.ExistsAsync(req.UserId))
            {
                HttpContext.Response.StatusCode = 404;
                AddError("User not found");
                return;
            }

            // Verify role exists
            var role = await RoleRepository.GetByIdAsync(req.RoleId);
            if (role == null)
            {
                HttpContext.Response.StatusCode = 404;
                AddError("Role not found");
                return;
            }

            // Assign role to user
            await RoleRepository.AddUserToRoleAsync(req.UserId, req.RoleId);
            
            HttpContext.Response.StatusCode = 200;
        }
    }
}
