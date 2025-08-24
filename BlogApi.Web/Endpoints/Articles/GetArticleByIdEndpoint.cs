using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles
{
    public class GetArticleByIdRequest
    {
        public int Id { get; set; }
    }

    public class GetArticleByIdEndpoint : Endpoint<GetArticleByIdRequest, ArticleDto>
    {
        public GetArticleByIdUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/articles/{id}");
            AllowAnonymous();
            Tags("Articles");
            Summary(s =>
            {
                s.Summary = "Get article by ID";
                s.Description = "Retrieves a specific article by its ID";
                s.Response<ArticleDto>(200, "Article found");
                s.Response(404, "Article not found");
            });
        }

        public override async Task HandleAsync(GetArticleByIdRequest req, CancellationToken ct)
        {
            var result = await UseCase.ExecuteAsync(req.Id);
            
            if (result == null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Response = result;
        }
    }
}