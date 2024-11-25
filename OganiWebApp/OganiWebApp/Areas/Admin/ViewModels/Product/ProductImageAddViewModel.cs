namespace OganiWebApp.Areas.Admin.ViewModels.Product
{
    public class ProductImageAddViewModel
    {
        public int ProductId { get; set; }
        public int Order { get; set; }
        public IFormFile? Image { get; set; }
    }
}
