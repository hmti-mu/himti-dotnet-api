using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class DeleteMediaUseCase
    {
        private readonly IMediaRepository _mediaRepository;

        public DeleteMediaUseCase(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            if (!await _mediaRepository.ExistsAsync(id))
            {
                return false;
            }

            await _mediaRepository.DeleteAsync(id);
            return true;
        }
    }
}