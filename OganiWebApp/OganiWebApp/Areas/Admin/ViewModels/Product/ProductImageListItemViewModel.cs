namespace OganiWebApp.Areas.Admin.ViewModels.Product
{
    public class ProductImageListItemViewModel
    {
        public int ProductId { get; set; }
        public List<ProductImageList> ProductImages { get; set; }
    }

    public class ProductImageList
    {
        public int ProductImageId { get; set; }
        public string ImageUrl { get; set; }
    }
}
