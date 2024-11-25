namespace OganiWebApp.Database.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string MainImageName { get; set; }
        public string MainImageNameInFileSystem { get; set; }
        public string Name { get; set; }
        public string Shipping { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public int RemindCount { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int PorductCategoryId { get; set; }
        public ProductCategory? PorductCategory { get; set; }
        public List<ProductSize>? ProductSizes { get; set; }
        public List<BasketProduct>? BasketProducts { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
