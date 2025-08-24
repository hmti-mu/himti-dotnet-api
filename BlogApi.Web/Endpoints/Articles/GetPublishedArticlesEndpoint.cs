using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles
{
    public class GetPublishedArticlesEndpoint : EndpointWithoutRequest<IEnumerable<ArticleDto>>
    {
        public GetPublishedArticlesUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/articles/published");
            AllowAnonymous(); // Public endpoint to get published articles
            Tags("Articles");
            Summary(s =>
            {
                s.Summary = "Get published articles";
                s.Description = "Retrieves all published articles. Public endpoint.";
                s.Response<IEnumerable<ArticleDto>>(200, "Published articles retrieved successfully");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var articles = await UseCase.ExecuteAsync();
            Response = articles;
        }
    }
}