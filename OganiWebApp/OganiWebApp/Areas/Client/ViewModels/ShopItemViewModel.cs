using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class ShopItemViewModel
    {
        public int Id { get; set; }
        public string MainImageUrl { get; set; }
        public List<string> Images { get; set; }
        public string Name { get; set; }
        public string Shipping { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public int RemindCount { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PorductCategoryId { get; set; }

        public List<ProductItemViewModel> RelatedProducts {  get; set; }    
    }
}
