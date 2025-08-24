using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Articles
{
    public class DeleteArticleRequest
    {
        public int Id { get; set; }
    }    public class DeleteArticleEndpoint : Endpoint<DeleteArticleRequest>
    {
        public DeleteArticleUseCase UseCase { get; set; } = null!;        public override void Configure()
        {
            Delete("/api/articles/{id}");
            Policies("RequireUser");
            Tags("1. Articles");
            Summary(s =>
            {
                s.Summary = "Delete an article";
                s.Description = "Deletes an existing article by its ID. Requires authentication.";
                s.Response(204, "Article deleted successfully");
                s.Response(404, "Article not found");
                s.Response(401, "Unauthorized - authentication required");
            });
        }

        public override async Task HandleAsync(DeleteArticleRequest req, CancellationToken ct)
        {
            var success = await UseCase.ExecuteAsync(req.Id);
            
            if (!success)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            HttpContext.Response.StatusCode = 204;
        }
    }
}

