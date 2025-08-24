using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Models.Requests;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles
{
    public class CreateArticleEndpoint : Endpoint<CreateArticleRequest, ArticleDto>
    {
        public CreateArticleUseCase UseCase { get; set; } = null!;        public override void Configure()
        {
            Post("/api/articles");
            AllowAnonymous(); // Temporary change for testing
            Tags("1. Articles");
            Summary(s =>
            {
                s.Summary = "Create a new article";
                s.Description = "Creates a new article with the provided title and content.";
                s.Response<ArticleDto>(201, "Article created successfully");
                s.Response(400, "Bad request - validation failed");
            });
        }public override async Task HandleAsync(CreateArticleRequest req, CancellationToken ct)
        {
            // Get author ID from the authenticated user's claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "user_id");
            int? authorId = null;
            
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                authorId = userId;
            }
            
            var result = await UseCase.ExecuteAsync(req.Title, req.Content, req.Category ?? "", req.ThumbnailUrl, authorId);
            Response = result;
            HttpContext.Response.StatusCode = 201;
        }
    }
}
