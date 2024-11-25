using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "FeuturedProduct")]
    public class FeuturedProduct : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public FeuturedProduct(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var product = _dataContext.Products
             .Include(p => p.OrderProducts).ToList();
            if (categoryId != 0)
            {
                product = _dataContext.Products
                    .Include(p => p.OrderProducts).Where(p => p.PorductCategoryId == categoryId).ToList();
            }

            product.ForEach(x => x.MainImageNameInFileSystem = _fileService.GetFileUrl(x.MainImageNameInFileSystem, UploadDirectory.Proudct));

            return View(product);
        }

    }
}
