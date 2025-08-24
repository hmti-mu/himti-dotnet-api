using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using FastEndpoints;

namespace BlogApi.Web.Endpoints.Media
{
    public class GetMediaEndpoint : EndpointWithoutRequest<IEnumerable<MediaDto>>
    {
        public GetMediaUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Get("/api/media");
            Policies("RequireUser");
            Tags("3. Media");
            Summary(s =>
            {
                s.Summary = "Get all media files";
                s.Description = "Retrieves a paginated list of media files. Requires authentication.";
                s.Response<IEnumerable<MediaDto>>(200, "Media files retrieved successfully");
                s.Response(401, "Unauthorized - authentication required");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Get pagination parameters from query string
            var pageQuery = Query<int?>("page", false);
            var pageSizeQuery = Query<int?>("pageSize", false);
            
            var page = pageQuery ?? 1;
            var pageSize = pageSizeQuery ?? 10;

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limit max page size

            var (media, totalCount) = await UseCase.ExecuteAsync(page, pageSize);
            Response = media;

            // Add pagination headers
            HttpContext.Response.Headers["X-Total-Count"] = totalCount.ToString();
            HttpContext.Response.Headers["X-Page"] = page.ToString();
            HttpContext.Response.Headers["X-Page-Size"] = pageSize.ToString();
        }
    }
}