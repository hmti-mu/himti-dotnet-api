using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Media
{
    public class DeleteMediaRequest
    {
        public int Id { get; set; }
    }

    public class DeleteMediaEndpoint : Endpoint<DeleteMediaRequest>
    {
        public DeleteMediaUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Delete("/api/media/{id}");
            Policies("RequireAdmin"); // Only admins can delete media
            Tags("Media");
            Summary(s =>
            {
                s.Summary = "Delete a media file";
                s.Description = "Deletes a media file. Requires admin privileges.";
                s.Response(204, "Media file deleted successfully");
                s.Response(404, "Media file not found");
                s.Response(401, "Unauthorized - authentication required");
                s.Response(403, "Forbidden - admin privileges required");
            });
        }

        public override async Task HandleAsync(DeleteMediaRequest req, CancellationToken ct)
        {
            var success = await UseCase.ExecuteAsync(req.Id);
            
            if (!success)
            {
                HttpContext.Response.StatusCode = 404;
                AddError("Media file not found");
                return;
            }

            HttpContext.Response.StatusCode = 204;
        }
    }
}