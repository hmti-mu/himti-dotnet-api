using BlogApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Web.Models.Requests
{    public class CreateArticleRequest
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
        public string Content { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        [Url(ErrorMessage = "ThumbnailUrl must be a valid URL")]
        public string? ThumbnailUrl { get; set; }

        // SEO fields
        [StringLength(200, ErrorMessage = "Slug cannot exceed 200 characters")]
        public string? Slug { get; set; }

        [StringLength(200, ErrorMessage = "MetaTitle cannot exceed 200 characters")]
        public string? MetaTitle { get; set; }

        [StringLength(300, ErrorMessage = "MetaDescription cannot exceed 300 characters")]
        public string? MetaDescription { get; set; }

        [StringLength(500, ErrorMessage = "MetaKeywords cannot exceed 500 characters")]
        public string? MetaKeywords { get; set; }

        // Status and workflow fields
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;
        
        public DateTime? ScheduledPublishAt { get; set; }
    }
}
