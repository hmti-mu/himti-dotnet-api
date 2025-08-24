using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class UpdateArticleUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public UpdateArticleUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<ArticleDto?> ExecuteAsync(int id, string title, string content)
        {
            var existingArticle = await _articleRepository.GetByIdAsync(id);
            if (existingArticle == null)
                return null;

            existingArticle.Title = title;
            existingArticle.Content = content;

            var updatedArticle = await _articleRepository.UpdateAsync(existingArticle);

            return new ArticleDto
            {
                Id = updatedArticle.Id,
                Title = updatedArticle.Title,
                Content = updatedArticle.Content,
                PublishedDate = updatedArticle.PublishedDate
            };
        }
    }
}
