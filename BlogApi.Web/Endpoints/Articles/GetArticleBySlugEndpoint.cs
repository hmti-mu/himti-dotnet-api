using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles
{
    public class GetArticleBySlugRequest
    {
        public string Slug { get; set; } = string.Empty;
    }

    public class GetArticleBySlugEndpoint : Endpoint<GetArticleBySlugRequest, ArticleDto>
    {
        public GetArticleBySlugUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/articles/slug/{slug}");
            AllowAnonymous(); // Public endpoint
            Tags("1. Articles");
            Summary(s =>
            {
                s.Summary = "Get article by slug";
                s.Description = "Retrieves a specific article by its SEO-friendly slug. Public endpoint.";
                s.Response<ArticleDto>(200, "Article retrieved successfully");
                s.Response(404, "Article not found");
            });
        }

        public override async Task HandleAsync(GetArticleBySlugRequest req, CancellationToken ct)
        {
            var article = await UseCase.ExecuteAsync(req.Slug);
            
            if (article == null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Response = article;
        }
    }
}