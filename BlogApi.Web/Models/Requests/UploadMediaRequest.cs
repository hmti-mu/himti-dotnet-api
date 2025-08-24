using System.ComponentModel.DataAnnotations;

namespace BlogApi.Web.Models.Requests
{
    public class UploadMediaRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
        
        public string? AltText { get; set; }
    }
}