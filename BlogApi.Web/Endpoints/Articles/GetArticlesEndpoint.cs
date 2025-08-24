using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles;

public class GetArticlesEndpoint : EndpointWithoutRequest<IEnumerable<ArticleDto>>
{
    private readonly GetArticlesUseCase _useCase;

    public GetArticlesEndpoint(GetArticlesUseCase useCase)
    {
        _useCase = useCase;
    }

    public override void Configure()
    {
        Get("/articles");
        AllowAnonymous();
        Description(d => d
            .WithTags("Articles")
            .WithSummary("Get all articles"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var articles = await _useCase.ExecuteAsync();
        Response = articles;
    }
}