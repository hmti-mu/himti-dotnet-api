using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Media
{
    public class GetMediaByIdRequest
    {
        public int Id { get; set; }
    }

    public class GetMediaByIdEndpoint : Endpoint<GetMediaByIdRequest, MediaDto>
    {
        public GetMediaUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/media/{id}");
            AllowAnonymous(); // Public endpoint for serving media
            Tags("3. Media");
            Summary(s =>
            {
                s.Summary = "Get media file by ID";
                s.Description = "Retrieves a specific media file by its ID. Public endpoint.";
                s.Response<MediaDto>(200, "Media file retrieved successfully");
                s.Response(404, "Media file not found");
            });
        }

        public override async Task HandleAsync(GetMediaByIdRequest req, CancellationToken ct)
        {
            var media = await UseCase.GetByIdAsync(req.Id);
            
            if (media == null)
            {
                HttpContext.Response.StatusCode = 404;
                AddError("Media file not found");
                return;
            }

            Response = media;
        }
    }
}