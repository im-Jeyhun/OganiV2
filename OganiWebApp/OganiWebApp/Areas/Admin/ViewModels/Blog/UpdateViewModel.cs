namespace OganiWebApp.Areas.Admin.ViewModels.Blog
{
    public class UpdateViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<OganiWebApp.Database.Models.BlogCategory>? BlogCategories { get; set; }
    }
}
