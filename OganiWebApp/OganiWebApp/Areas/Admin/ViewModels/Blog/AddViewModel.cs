using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Admin.ViewModels.Blog
{
    public class AddViewModel
    {
        public IFormFile Image { get; set; }
        public int BlogCategoryId { get; set; }

        public List<OganiWebApp.Database.Models.BlogCategory>? BlogCategories { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
