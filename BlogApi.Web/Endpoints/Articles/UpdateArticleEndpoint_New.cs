using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Web.Endpoints.Articles
{
    public class UpdateArticleRequestModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateArticleEndpoint : Endpoint<UpdateArticleRequestModel, ArticleDto>
    {
        public UpdateArticleUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Put("/api/articles/{id}");
            AllowAnonymous();
            Tags("Articles");
            Summary(s =>
            {
                s.Summary = "Update an article";
                s.Description = "Updates an existing article with new title and content";
                s.Response<ArticleDto>(200, "Article updated successfully");
                s.Response(404, "Article not found");
                s.Response(400, "Bad request - validation failed");
            });
        }

        public override async Task HandleAsync(UpdateArticleRequestModel req, CancellationToken ct)
        {
            var result = await UseCase.ExecuteAsync(req.Id, req.Title, req.Content);
            
            if (result == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            Response = result;
        }
    }
}
