using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class GetMediaUseCase
    {
        private readonly IMediaRepository _mediaRepository;

        public GetMediaUseCase(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<(IEnumerable<MediaDto> Media, int TotalCount)> ExecuteAsync(int page = 1, int pageSize = 10)
        {
            var mediaList = await _mediaRepository.GetAllAsync(page, pageSize);
            var totalCount = mediaList.Count(); // For now, we'll need to improve this for actual pagination

            var mediaDtos = mediaList.Select(m => new MediaDto
            {
                Id = m.Id,
                FileName = m.FileName,
                FilePath = m.FilePath,
                MimeType = m.MimeType,
                FileSize = m.FileSize,
                AltText = m.AltText,
                UploadedAt = m.UploadedAt,
                UploadedByUserId = m.UploadedByUserId,
                UploadedByUsername = m.UploadedBy?.Username
            });

            return (mediaDtos, totalCount);
        }

        public async Task<MediaDto?> GetByIdAsync(int id)
        {
            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null) return null;

            return new MediaDto
            {
                Id = media.Id,
                FileName = media.FileName,
                FilePath = media.FilePath,
                MimeType = media.MimeType,
                FileSize = media.FileSize,
                AltText = media.AltText,
                UploadedAt = media.UploadedAt,
                UploadedByUserId = media.UploadedByUserId,
                UploadedByUsername = media.UploadedBy?.Username
            };
        }
    }
}