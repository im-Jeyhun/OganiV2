namespace OganiWebApp.Database.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string ThumbNailImgName { get; set; }
        public string ThumbNailImgNameInFileSystem { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogCategory? BlogCategory { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
