using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IFileService _fileService;
        public BlogController(DataContext dbContext, IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }

        [HttpGet("index", Name = "client-blog-index")]
        public IActionResult Index(int? CategoryId = null, string Search = null)
        {
            var blogs = _dbContext.Blogs
                .Include(x => x.User)
                .Include(X=> X.BlogCategory)
               .AsEnumerable();
            if (CategoryId != null)
            {
                blogs = blogs.Where(b => b.BlogCategoryId == CategoryId);

            }
            if (Search != null)
            {
                blogs = blogs.Where(b => b.Title.Contains(Search) ||
                                    b.Content.Contains(Search));
            }
            var blogModel = blogs.Select(x =>
                                               new BlogViewModel(x.Id, x.ThumbNailImgName,
                                                                 _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog),
                                                                 x.User, x.BlogCategoryId, x.BlogCategory, x.Title, x.Content, x.CreatedAt)).ToList();

            var relatedBlogs = _dbContext.Blogs.Include(x=> x.User).Include(x=> x.BlogCategory).Select(x =>
                                               new BlogViewModel(x.Id, x.ThumbNailImgName,
                                                                 _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog),
                                                                 x.User, x.BlogCategoryId, x.BlogCategory, x.Title, x.Content, x.CreatedAt)).ToList();
            var model = new BlogListViewModel
            {
                BlogCategories = _dbContext.BlogCategories.ToList(),
                Blogs = blogModel,
                NewBlogs = relatedBlogs
            };
            return View(model);
        }
        [HttpGet("blog-filter", Name = "client-blog-filter")]
        public async Task<IActionResult> FilterAsync( string Search = null , int? CategoryId = null)
        {
            return RedirectToRoute("client-blog-index", new { Search = Search , CategoryId = CategoryId});
        }
        [HttpGet("blog-item/{id}", Name = "client-blog-item")]
        public async Task<IActionResult> Item(int? id)
        {
            var blog = _dbContext.Blogs.Include(x => x.User).Include(x=> x.BlogCategory).FirstOrDefault(x => x.Id == id);
            if (blog == null) return NotFound();
            var blogCategories = _dbContext.BlogCategories.ToList();
            var model = new BlogItemVIewModel
            {
                Blog = new BlogViewModel(blog.Id, blog.ThumbNailImgName, _fileService.GetFileUrl(blog.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog), blog.User, blog.BlogCategoryId, blog.BlogCategory, blog.Title, blog.Content, blog.CreatedAt),
                BlogCategories = blogCategories,
                SuggestedBlogs = _dbContext.Blogs.Select(x => new BlogViewModel(x.Id, x.ThumbNailImgName, _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog), x.User, x.BlogCategoryId, x.BlogCategory, x.Title, x.Content, x.CreatedAt)).ToList(),
                NewBlogs = _dbContext.Blogs.Select(x => new BlogViewModel(x.Id, x.ThumbNailImgName, _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog), x.User, x.BlogCategoryId, x.BlogCategory, x.Title, x.Content, x.CreatedAt)).ToList()
            };
            return View(model);
        }

    }
}
