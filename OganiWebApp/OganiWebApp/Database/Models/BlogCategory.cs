namespace OganiWebApp.Database.Models
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
