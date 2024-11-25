namespace OganiWebApp.Areas.Admin.ViewModels.Product
{
    public class ProductImageUpdateViewModel
    {
        public int ProductId { get; set; }
        public int ProductImgId { get; set; }
        public int Order { get; set; }

        public string ImageUrl { get; set; }    
        public IFormFile? Image { get; set; }
    }
}
