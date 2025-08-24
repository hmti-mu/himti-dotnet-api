using BlogApi.Application.DTOs;
using BlogApi.Application.UseCases;
using BlogApi.Web.Models.Requests;
using FastEndpoints;
using System.Security.Claims;

namespace BlogApi.Web.Endpoints.Media
{
    public class UploadMediaEndpoint : Endpoint<UploadMediaRequest, MediaDto>
    {
        public UploadMediaUseCase UseCase { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/media/upload");
            Policies("RequireUser");
            AllowFormData(); // Enable multipart/form-data support
            Tags("3. Media");
            Summary(s =>
            {
                s.Summary = "Upload a media file";
                s.Description = "Uploads a media file to the server. Requires authentication.";
                s.Response<MediaDto>(201, "Media uploaded successfully");
                s.Response(400, "Bad request - invalid file or validation failed");
                s.Response(401, "Unauthorized - authentication required");
                s.Response(413, "Payload too large - file size exceeds limit");
            });
        }

        public override async Task HandleAsync(UploadMediaRequest req, CancellationToken ct)
        {
            // Get user ID from claims
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                HttpContext.Response.StatusCode = 401;
                AddError("User authentication required");
                return;
            }

            // Validate file
            var file = req.File;
            if (file == null || file.Length == 0)
            {
                HttpContext.Response.StatusCode = 400;
                AddError("No file was uploaded");
                return;
            }

            // File size limit (10MB)
            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                HttpContext.Response.StatusCode = 413;
                AddError("File size exceeds the maximum allowed size of 10MB");
                return;
            }

            // Validate file type
            var allowedMimeTypes = new[]
            {
                "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp",
                "application/pdf", "text/plain", "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            if (!allowedMimeTypes.Contains(file.ContentType))
            {
                HttpContext.Response.StatusCode = 400;
                AddError("File type not allowed. Supported types: images (JPEG, PNG, GIF, WebP), PDF, TXT, DOC, DOCX");
                return;
            }

            // Create uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Generate unique file name to prevent conflicts
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            // Save media info to database
            var result = await UseCase.ExecuteAsync(
                file.FileName,
                $"/uploads/{uniqueFileName}",
                file.ContentType,
                file.Length,
                req.AltText,
                userId);

            Response = result;
            HttpContext.Response.StatusCode = 201;
        }
    }
}