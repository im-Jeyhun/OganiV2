namespace OganiWebApp.Areas.Client.ViewModels
{
    public class ProductCookieViewModel
    {
        public ProductCookieViewModel(int id, string? name, string? imageUrl, int? quantity, decimal? price, decimal? discountPrice, decimal? total)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Price = price;
            DiscountPrice = discountPrice;
            Total = total;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public decimal? Total { get; set; }
    }
}
