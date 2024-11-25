namespace OganiWebApp.Database.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string BgImageName { get; set; }
        public string BgImageNameInFileSystem { get; set; }
        public int PorductCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
