using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class GetArticleByIdUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticleByIdUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<ArticleDto?> ExecuteAsync(int id)
        {
            var article = await _articleRepository.GetByIdAsync(id);

            if (article == null)
                return null;

            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Category = article.Category,
                ThumbnailUrl = article.ThumbnailUrl,
                PublishedDate = article.PublishedDate,
                AuthorId = article.AuthorId,
                AuthorName = article.Author?.Username,
                Slug = article.Slug,
                MetaTitle = article.MetaTitle,
                MetaDescription = article.MetaDescription,
                MetaKeywords = article.MetaKeywords,
                Status = article.Status,
                ScheduledPublishAt = article.ScheduledPublishAt,
                CreatedAt = article.CreatedAt,
                UpdatedAt = article.UpdatedAt
            };
        }
    }
}
