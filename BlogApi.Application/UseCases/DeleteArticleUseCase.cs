using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class DeleteArticleUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public DeleteArticleUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            var exists = await _articleRepository.ExistsAsync(id);
            if (!exists)
                return false;

            await _articleRepository.DeleteAsync(id);
            return true;
        }
    }
}
