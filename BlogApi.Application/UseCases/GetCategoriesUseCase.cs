using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class GetCategoriesUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public GetCategoriesUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<string>> ExecuteAsync()
        {
            return await _articleRepository.GetCategoriesAsync();
        }
    }
}
