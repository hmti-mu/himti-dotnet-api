namespace BlogApi.Domain.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? AltText { get; set; }
        public DateTime UploadedAt { get; set; }
        public int UploadedByUserId { get; set; }

        // Navigation properties
        public User? UploadedBy { get; set; }
    }
}