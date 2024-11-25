using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class BlogItemVIewModel
    {
        public string? Search {  get; set; }
        public int? BlogCategoryId { get; set; }
        public BlogViewModel Blog { get; set; }
        public List<BlogViewModel> NewBlogs { get; set; }
        public List<BlogViewModel> SuggestedBlogs { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
    }
}
