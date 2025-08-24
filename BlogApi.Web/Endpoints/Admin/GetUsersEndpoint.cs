using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Authorization;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Admin
{
    public class GetUsersEndpoint : EndpointWithoutRequest<IEnumerable<UserDto>>
    {
        public GetUsersUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/admin/users");
            Policies(PolicyNames.RequireAdmin);
            Tags("Administration");
            Summary(s =>
            {
                s.Summary = "Get all users";
                s.Description = "Retrieves a list of all users in the system (Admin only)";
                s.Response<IEnumerable<UserDto>>(200, "Users retrieved successfully");
                s.Response(401, "Unauthorized - user not authenticated");
                s.Response(403, "Forbidden - insufficient permissions");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = await UseCase.ExecuteAsync();
            Response = users;
        }
    }
}