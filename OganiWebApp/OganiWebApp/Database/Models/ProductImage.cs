namespace OganiWebApp.Database.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImageNameInFileSystem { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Order { get; set; }
    }
}
