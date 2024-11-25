using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class ShopViewModel
    {
        public string? Search { get; set; }
        public int? SizeId { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public List<ProductCategory>? ProductCategories { get; set; }
        public List<Size>? ProductSizes { get; set; }
        public List<ProductListViewModel>? DiscountedProducts { get; set; }
        public List<ProductListViewModel>? Products { get; set; }

        
    }
}
