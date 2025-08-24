using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles;

public class GetCategoriesEndpoint : EndpointWithoutRequest<IEnumerable<string>>
{
    public GetCategoriesUseCase UseCase { get; set; } = null!;

    public override void Configure()
    {
        Get("/api/articles/categories");
        AllowAnonymous();
        Tags("Articles");
        Summary(s =>
        {
            s.Summary = "Get all article categories";
            s.Description = "Retrieves a list of all available article categories";
            s.Response<IEnumerable<string>>(200, "Successfully retrieved categories");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categories = await UseCase.ExecuteAsync();
        Response = categories;
    }
}
