using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Areas.Admin.ViewModels.BlogCategory;
using OganiWebApp.Database.Models;
using OganiWebApp.Database;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/blog-category")]
    //[Authorize(Roles = "admin")]
    public class BlogCategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public BlogCategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-blog-category-list")]
        public IActionResult List()
        {
            var blogCategoryVM = _dataContext.BlogCategories.Select(x => new ListItemViewModel(x.Id, x.Title)).ToList();
            return View(blogCategoryVM);
        }
        [HttpGet("add-blog-category", Name = "admin-blog-category-add")]
        public IActionResult Add()
        {
            return View(new AddViewModel());
        }

        [HttpPost("add-blog-category", Name = "admin-blog-category-add")]
        public IActionResult Add(AddViewModel addViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addViewModel);
            }
            var blogCategory = new BlogCategory { Title = addViewModel.Title, CreatedAt = DateTime.Now };
            _dataContext.BlogCategories.Add(blogCategory);
            _dataContext.SaveChanges(); 
            return RedirectToRoute("admin-blog-category-list");
        }

        [HttpGet("update-blog-category/{id}", Name = "admin-blog-category-update")]
        public IActionResult Update(int id)
        {
            var blogCategory = _dataContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (blogCategory is null) return NotFound();
            return View(new UpdateViewModel { Id = blogCategory.Id, Title = blogCategory.Title });
        }

        [HttpPost("update-blog-category/{id}", Name = "admin-blog-category-update")]
        public IActionResult Update(int id, UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateViewModel);
            }
            var blogCategory = _dataContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (blogCategory is null) return NotFound();

            blogCategory.Title = updateViewModel.Title;

            _dataContext.SaveChanges();

            return RedirectToRoute("admin-blog-category-list");
        }

        [HttpPost("delete-blog-category/{id}", Name = "admin-blog-category-delete")]
        public IActionResult Delete(int id, UpdateViewModel updateViewModel)
        {
            var blogCategory = _dataContext.BlogCategories.FirstOrDefault(x => x.Id == id);
            if (blogCategory is null) return NotFound();

            _dataContext.BlogCategories.Remove(blogCategory);
            _dataContext.SaveChanges();

            return RedirectToRoute("admin-blog-category-list");
        }
    }
}
