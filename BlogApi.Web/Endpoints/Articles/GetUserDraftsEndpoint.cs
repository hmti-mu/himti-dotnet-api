using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;
using System.Security.Claims;

namespace BlogApi.Web.Endpoints.Articles
{
    public class GetUserDraftsEndpoint : EndpointWithoutRequest<IEnumerable<ArticleDto>>
    {
        public GetUserDraftsUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/articles/drafts");
            Policies("RequireUser"); // Authenticated users can see their drafts
            Tags("Articles");
            Summary(s =>
            {
                s.Summary = "Get user's draft articles";
                s.Description = "Retrieves draft articles for the authenticated user. Requires authentication.";
                s.Response<IEnumerable<ArticleDto>>(200, "Draft articles retrieved successfully");
                s.Response(401, "Unauthorized - authentication required");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Get user ID from claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                HttpContext.Response.StatusCode = 401;
                return;
            }

            var articles = await UseCase.ExecuteAsync(userId);
            Response = articles;
        }
    }
}