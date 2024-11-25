using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class IndexViewModel
    {
        public List<ProductCategoryViewModel>? Categories { get; set; }
        public List<SliderViewModel>? Sliders { get; set; }
        public List<List<ProductListViewModel>>? Products { get; set; }
        public List<BlogViewModel> Blogs { get; set; }

    }
}
