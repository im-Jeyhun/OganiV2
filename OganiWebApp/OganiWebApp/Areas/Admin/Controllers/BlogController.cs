using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Admin.ViewModels.Blog;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;
using OganiWebApp.Services;
using System.Reflection.Metadata;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/blog")]
    //[Authorize(Roles = "admin")]
    public class BlogController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public BlogController(DataContext dataContext, IFileService fileService, IUserService userService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
            _userService = userService;
        }
        [HttpGet("list", Name = "admin-blog-list")]
        public async Task<IActionResult> ListAsync()

        {
            var blogs = await _dataContext.Blogs
                .Include(p => p.BlogCategory)
                .Include(p => p.User).ToListAsync();

            var blogModel = _dataContext.Blogs.Select(x => new ListItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = _fileService.GetFileUrl(x.ThumbNailImgNameInFileSystem, Contracts.UploadDirectory.Blog)
            }).ToList();

            return View(blogModel);
        }
        [HttpGet("add-blog", Name = "admin-blog-add")]
        public async Task<IActionResult> AddAsync()
        {
            var categories = _dataContext.BlogCategories.ToList();

            return View(new AddViewModel { BlogCategories = categories });
        }
        [HttpPost("add-blog", Name = "admin-blog-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            var categories = _dataContext.BlogCategories.ToList();

            if (!ModelState.IsValid) return GetView();

            if (!_dataContext.BlogCategories.Any(c => c.Id == model.BlogCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Blog category is not found");
                return GetView();
            }
            var blog = new Blog
            {
                Title = model.Title,
                Content = model.Content,
                BlogCategoryId = model.BlogCategoryId,
                ThumbNailImgName = model.Image != null ? model.Image.FileName : default!,
                ThumbNailImgNameInFileSystem = model.Image != null ? await CreateBlogThumbNailImage() : default!,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _dataContext.Blogs.Add(blog);


            async Task<string> CreateBlogThumbNailImage()
            {
                return await _fileService.UploadAsync(model.Image, UploadDirectory.Blog);
            }
            IActionResult GetView()
            {
                model.BlogCategories = categories;
                return View(model);
            }

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-list");
        }

        [HttpGet("uptade-blog/{id}", Name = "admin-blog-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var blog = await _dataContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            var model = new UpdateViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                ImageUrl = _fileService.GetFileUrl(blog.ThumbNailImgNameInFileSystem, UploadDirectory.Blog),
                BlogCategoryId = blog.BlogCategoryId,
                BlogCategories = _dataContext.BlogCategories.ToList(),

            };

            return View(model);
        }

        [HttpPost("uptade-blog/{id}", Name = "admin-blog-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateViewModel model)
        {
            var categories = _dataContext.BlogCategories.ToList();
            var blog = await _dataContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (!ModelState.IsValid) return GetView();

            if (blog is null) return NotFound();

            if (!_dataContext.BlogCategories.Any(c => c.Id == model.BlogCategoryId))
            {
                ModelState.AddModelError(string.Empty, "Blog category is not found");
                return GetView();
            }
            blog.Title = model.Title;
            blog.Content = model.Content;
            blog.UpdatedAt = DateTime.Now;
            blog.User = _userService.CurrentUser;
            blog.UserId = _userService.CurrentUser.Id;
            blog.BlogCategoryId = model.BlogCategoryId;

            if (model.Image != null)
            {
                await _fileService.DeleteAsync(blog.ThumbNailImgNameInFileSystem, UploadDirectory.Blog);
                var fileName = await _fileService.UploadAsync(model.Image, UploadDirectory.Blog);
                blog.ThumbNailImgNameInFileSystem = fileName;
                blog.ThumbNailImgName = model.Image.FileName;
            }

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-blog-list");

            IActionResult GetView()
            {
                model.BlogCategories = categories;
                return View(model);
            }
        }

        [HttpPost("delete-blog/{id}", Name = "admin-blog-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var blog = await _dataContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null) return NotFound();

            _dataContext.Blogs.Remove(blog);

            await _dataContext.SaveChangesAsync();

            await _fileService.DeleteAsync(blog.ThumbNailImgNameInFileSystem, UploadDirectory.Blog);
            return RedirectToRoute("admin-add-blog-list");

        }

    }
}
