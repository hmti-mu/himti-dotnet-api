using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class GetUserDraftsUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public GetUserDraftsUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<ArticleDto>> ExecuteAsync(int userId)
        {
            var articles = await _articleRepository.GetDraftsByUserAsync(userId);
            var articleDtos = new List<ArticleDto>();
            
            foreach (var article in articles)
            {
                articleDtos.Add(new ArticleDto
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    Category = article.Category,
                    ThumbnailUrl = article.ThumbnailUrl,
                    PublishedDate = article.PublishedDate,
                    AuthorName = article.Author?.Username,
                    AuthorId = article.AuthorId,
                    Slug = article.Slug,
                    MetaTitle = article.MetaTitle,
                    MetaDescription = article.MetaDescription,
                    MetaKeywords = article.MetaKeywords,
                    Status = article.Status,
                    ScheduledPublishAt = article.ScheduledPublishAt,
                    CreatedAt = article.CreatedAt,
                    UpdatedAt = article.UpdatedAt
                });
            }
            
            return articleDtos;
        }
    }
}