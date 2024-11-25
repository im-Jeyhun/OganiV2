using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Database;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.ViewComponents
{
    [ViewComponent(Name = "Header")]
    public class Header : ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public Header(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _dataContext.ProductCategories.ToList();
            return View(categories);
        }
    }
}
