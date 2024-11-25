using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Client.ViewModels
{
    public class ProductCategoryViewModel
    {
        public ProductCategoryViewModel(int id, string title)
        {
            Id = id;
            Title = title;
        }
        public int Id { get; set; }
        public string Title { get; set; }

    }
}
