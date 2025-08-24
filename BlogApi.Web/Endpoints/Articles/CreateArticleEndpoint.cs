using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Authorization;
using BlogApi.Web.Models.Requests;
using FastEndpoints;
using System.Security.Claims;

namespace BlogApi.Web.Endpoints.Articles
{
    public class CreateArticleEndpoint : Endpoint<CreateArticleRequest, ArticleDto>
    {
        public CreateArticleUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/articles");
            Policies(PolicyNames.RequireUser);
            Tags("Articles");
            Summary(s =>
            {
                s.Summary = "Create a new article";
                s.Description = "Creates a new article with the provided title and content (requires authentication)";
                s.Response<ArticleDto>(201, "Article created successfully");
                s.Response(400, "Bad request - validation failed");
                s.Response(401, "Unauthorized - user not authenticated");
            });
        }

        public override async Task HandleAsync(CreateArticleRequest req, CancellationToken ct)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var authorId = userIdClaim != null && int.TryParse(userIdClaim.Value, out var id) ? id : (int?)null;

            var result = await UseCase.ExecuteAsync(req.Title, req.Content, authorId);
            Response = result;
            HttpContext.Response.StatusCode = 201;
        }
    }
}