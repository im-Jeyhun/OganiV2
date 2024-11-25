using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class ProductListViewModel
    {
        public ProductListViewModel(int id, string mainImageName, string mainImageUrl, string name, string shipping, decimal weight, string description, int remindCount, decimal price, decimal? discountPrice, int porductCategoryId)
        {
            Id = id;
            MainImageName = mainImageName;
            MainImageUrl = mainImageUrl;
            Name = name;
            Shipping = shipping;
            Weight = weight;
            Description = description;
            RemindCount = remindCount;
            Price = price;
            DiscountPrice = discountPrice;
            PorductCategoryId = porductCategoryId;
        }

        public int Id { get; set; }
        public string MainImageName { get; set; }
        public string MainImageUrl { get; set; }
        public string Name { get; set; }
        public string Shipping { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public int RemindCount { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int PorductCategoryId { get; set; }

    }
}
