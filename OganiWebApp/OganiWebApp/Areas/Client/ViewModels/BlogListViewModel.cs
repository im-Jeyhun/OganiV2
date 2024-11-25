using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class BlogListViewModel
    {
        public string Search { get; set; }
        public int BlogCategoryId { get; set; }
        public List<BlogViewModel> Blogs { get; set; }
        public List<BlogViewModel> NewBlogs { get; set; }
        public List<BlogCategory> BlogCategories { get; set; }
    }
}
