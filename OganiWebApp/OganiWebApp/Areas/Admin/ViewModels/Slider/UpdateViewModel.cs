using System.ComponentModel.DataAnnotations;

namespace OganiWebApp.Areas.Admin.ViewModels.Slider
{
    public class UpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public IFormFile BgImage { get; set; }

        [Required]
        public int PorductCategoryId { get; set; }
        public List<OganiWebApp.Database.Models.ProductCategory>? ProductCategories { get; set; }

    }
}
