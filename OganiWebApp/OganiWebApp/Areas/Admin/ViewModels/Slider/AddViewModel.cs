using System.ComponentModel.DataAnnotations;

namespace OganiWebApp.Areas.Admin.ViewModels.Slider
{
    public class AddViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public IFormFile BgImage { get; set; }

        [Required]
        public int PorductCategoryId { get; set; }

        public List<OganiWebApp.Database.Models.ProductCategory>? ProductCategories { get; set; }
    }
}
