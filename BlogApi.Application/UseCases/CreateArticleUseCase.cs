using BlogApi.Application.DTOs;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class CreateArticleUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public CreateArticleUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }        public async Task<ArticleDto> ExecuteAsync(string title, string content, string category = "", string? thumbnailUrl = null, int? authorId = null)
        {
            var article = new Article
            {
                Title = title,
                Content = content,
                Category = category,
                ThumbnailUrl = thumbnailUrl,
                AuthorId = authorId
            };

            var createdArticle = await _articleRepository.CreateAsync(article);

            return new ArticleDto
            {
                Id = createdArticle.Id,
                Title = createdArticle.Title,
                Content = createdArticle.Content,
                Category = createdArticle.Category,
                ThumbnailUrl = createdArticle.ThumbnailUrl,
                PublishedDate = createdArticle.PublishedDate,
                AuthorId = createdArticle.AuthorId
            };
        }
    }
}
