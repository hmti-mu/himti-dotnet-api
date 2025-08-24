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
    }
}
