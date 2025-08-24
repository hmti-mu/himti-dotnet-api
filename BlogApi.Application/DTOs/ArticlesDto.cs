using BlogApi.Domain.Enums;

namespace BlogApi.Application.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public string? AuthorName { get; set; }
        public int? AuthorId { get; set; }
        
        // SEO fields
        public string? Slug { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        // Status and workflow fields
        public ArticleStatus Status { get; set; }
        public DateTime? ScheduledPublishAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ArticleSearchFilterDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public int? AuthorId { get; set; }
        public DateTime? PublishedDateFrom { get; set; }
        public DateTime? PublishedDateTo { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ArticleSearchResultDto
    {
        public IEnumerable<ArticleDto> Articles { get; set; } = new List<ArticleDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
