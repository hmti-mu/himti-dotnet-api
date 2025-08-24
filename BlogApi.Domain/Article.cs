namespace BlogApi.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public int? AuthorId { get; set; } // Nullable to support existing articles without authors

        // Navigation properties
        public User? Author { get; set; }
    }
}
