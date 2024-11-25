namespace OganiWebApp.Database.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
