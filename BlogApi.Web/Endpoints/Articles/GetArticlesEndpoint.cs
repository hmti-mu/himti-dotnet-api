using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles;

public class GetArticlesEndpoint : EndpointWithoutRequest<IEnumerable<ArticleDto>>
{
    public GetArticlesUseCase UseCase { get; set; } = null!;

    public override void Configure()
    {
        Get("/api/articles");
        AllowAnonymous();
        Tags("Articles");
        Summary(s =>
        {
            s.Summary = "Get all articles";
            s.Description = "Retrieves a list of all articles";
            s.Response<IEnumerable<ArticleDto>>(200, "Successfully retrieved articles");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var articles = await UseCase.ExecuteAsync();
        Response = articles;
    }
}