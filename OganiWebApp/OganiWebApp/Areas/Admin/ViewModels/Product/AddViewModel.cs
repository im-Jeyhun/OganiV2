using OganiWebApp.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace OganiWebApp.Areas.Admin.ViewModels.Product
{
    public class AddViewModel
    {
        [Required]
        public IFormFile MainImage { get; set; }
        public string Name { get; set; }
        public string Shipping { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public int RemindCount { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int PorductCategoryId { get; set; }
        public List<OganiWebApp.Database.Models.ProductCategory>? ProductCategories { get; set; }
        public List<int>? SizeIds { get; set; }
        public List<OganiWebApp.Database.Models.Size>? ProductSizes { get; set; }
        [Required]
        public List<IFormFile>? Images { get; set; }
    }
}
