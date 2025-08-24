using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles;

public class SearchArticlesRequest
{
    public string? SearchTerm { get; set; }
    public string? Category { get; set; }
    public int? AuthorId { get; set; }
    public DateTime? PublishedDateFrom { get; set; }
    public DateTime? PublishedDateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchArticlesEndpoint : Endpoint<SearchArticlesRequest, ArticleSearchResultDto>
{
    public SearchArticlesUseCase UseCase { get; set; } = null!;

    public override void Configure()
    {
        Get("/api/articles/search");
        AllowAnonymous();
        Tags("Articles");
        Summary(s =>
        {
            s.Summary = "Search and filter articles";
            s.Description = "Search articles by keyword and filter by category, author, and date range with pagination";
            s.Response<ArticleSearchResultDto>(200, "Successfully retrieved filtered articles");
        });
    }

    public override async Task HandleAsync(SearchArticlesRequest req, CancellationToken ct)
    {
        var filter = new ArticleSearchFilterDto
        {
            SearchTerm = req.SearchTerm,
            Category = req.Category,
            AuthorId = req.AuthorId,
            PublishedDateFrom = req.PublishedDateFrom,
            PublishedDateTo = req.PublishedDateTo,
            Page = req.Page,
            PageSize = req.PageSize
        };

        var result = await UseCase.ExecuteAsync(filter);
        Response = result;
    }
}
