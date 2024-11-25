using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IFileService _fileService;
        public HomeController(DataContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet("~/", Name = "client-home-index")]
        [HttpGet("index")]
        public IActionResult Index()
        {

            var indexViewModel = new IndexViewModel
            {
                Categories = _dbContext.ProductCategories.Select(x => new ProductCategoryViewModel(x.Id, x.Title)).ToList(),
                Sliders = _dbContext.Sliders.Select(x => new SliderViewModel(x.Title, x.BgImageName, _fileService.GetFileUrl(x.BgImageNameInFileSystem,Contracts.UploadDirectory.Slider), x.PorductCategoryId)).ToList(),
                Products = _dbContext.Products.ToList().Select((entity, index) => new { entity, index })
                                        .GroupBy(x => x.index / 3)
                                        .Select(g => g.Select(x => new ProductListViewModel(x.entity.Id , 
                                                                                            x.entity.MainImageName ,
                                                                                            _fileService.GetFileUrl(x.entity.MainImageNameInFileSystem,Contracts.UploadDirectory.Proudct),
                                                                                            x.entity.Name,
                                                                                            x.entity.Shipping , 
                                                                                            x.entity.Weight , 
                                                                                            x.entity.Description , 
                                                                                            x.entity.RemindCount ,
                                                                                            x.entity.Price,
                                                                                            x.entity.DiscountPrice,
                                                                                            x.entity.PorductCategoryId
                                                                                            )).ToList())
                                        .ToList(),
                Blogs = _dbContext.Blogs.Take(3).Select(x => new BlogViewModel(x.Id ,
                                                                               x.ThumbNailImgName , 
                                                                               _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem ,
                                                                               Contracts.UploadDirectory.Blog),
                                                                               x.User ,
                                                                               x.BlogCategoryId , 
                                                                               _dbContext.BlogCategories.FirstOrDefault(bc => bc.Id == x.BlogCategoryId),
                                                                               x.Title,
                                                                               x.Content,
                                                                               x.CreatedAt)).ToList()
            };
            return View(indexViewModel);
        }
    }
}
