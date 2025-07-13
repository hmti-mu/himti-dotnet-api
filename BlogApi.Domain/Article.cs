namespace BlogApi.Domain.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public int Content { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
