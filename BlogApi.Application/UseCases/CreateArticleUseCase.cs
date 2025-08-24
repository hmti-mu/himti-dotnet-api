using BlogApi.Application.DTOs;
using BlogApi.Application.Utilities;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class CreateArticleUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public CreateArticleUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }        public async Task<ArticleDto> ExecuteAsync(string title, string content, string category = "", string? thumbnailUrl = null, int? authorId = null, string? slug = null, string? metaTitle = null, string? metaDescription = null, string? metaKeywords = null, ArticleStatus status = ArticleStatus.Draft, DateTime? scheduledPublishAt = null)
        {
            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(slug))
            {
                slug = SlugGenerator.GenerateSlug(title);
            }
            else
            {
                // Validate provided slug
                if (!SlugGenerator.IsValidSlug(slug))
                {
                    throw new ArgumentException("Invalid slug format. Slug must contain only lowercase letters, numbers, hyphens, and underscores.");
                }
            }

            // Ensure slug is unique
            if (!string.IsNullOrWhiteSpace(slug) && await _articleRepository.SlugExistsAsync(slug))
            {
                // Append a number to make it unique
                var baseSlug = slug;
                var counter = 1;
                while (await _articleRepository.SlugExistsAsync(slug))
                {
                    slug = $"{baseSlug}-{counter}";
                    counter++;
                }
            }

            var now = DateTime.UtcNow;
            var article = new Article
            {
                Title = title,
                Content = content,
                Category = category,
                ThumbnailUrl = thumbnailUrl,
                AuthorId = authorId,
                Slug = slug,
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords,
                Status = status,
                ScheduledPublishAt = scheduledPublishAt,
                CreatedAt = now,
                UpdatedAt = now,
                PublishedDate = status == ArticleStatus.Published ? now : (scheduledPublishAt ?? now)
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
                AuthorId = createdArticle.AuthorId,
                Slug = createdArticle.Slug,
                MetaTitle = createdArticle.MetaTitle,
                MetaDescription = createdArticle.MetaDescription,
                MetaKeywords = createdArticle.MetaKeywords,
                Status = createdArticle.Status,
                ScheduledPublishAt = createdArticle.ScheduledPublishAt,
                CreatedAt = createdArticle.CreatedAt,
                UpdatedAt = createdArticle.UpdatedAt
            };
        }
    }
}
