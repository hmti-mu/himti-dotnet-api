using BlogApi.Application.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class UploadMediaUseCase
    {
        private readonly IMediaRepository _mediaRepository;

        public UploadMediaUseCase(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<MediaDto> ExecuteAsync(string fileName, string filePath, string mimeType, long fileSize, string? altText, int uploadedByUserId)
        {
            var media = new Media
            {
                FileName = fileName,
                FilePath = filePath,
                MimeType = mimeType,
                FileSize = fileSize,
                AltText = altText,
                UploadedAt = DateTime.UtcNow,
                UploadedByUserId = uploadedByUserId
            };

            var createdMedia = await _mediaRepository.CreateAsync(media);

            return new MediaDto
            {
                Id = createdMedia.Id,
                FileName = createdMedia.FileName,
                FilePath = createdMedia.FilePath,
                MimeType = createdMedia.MimeType,
                FileSize = createdMedia.FileSize,
                AltText = createdMedia.AltText,
                UploadedAt = createdMedia.UploadedAt,
                UploadedByUserId = createdMedia.UploadedByUserId
            };
        }
    }
}