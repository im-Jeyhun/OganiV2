using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Areas.Admin.ViewModels.ProductCategory;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/product-category")]
    //[Authorize(Roles = "admin")]
    public class ProductCategoryController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductCategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-product-category-list")]
        public IActionResult List()
        {
            var productCategoryVM = _dataContext.ProductCategories.Select(x => new ListItemViewModel(x.Id, x.Title)).ToList();
            return View(productCategoryVM);
        }
        [HttpGet("add-category", Name = "admin-product-category-add")]
        public IActionResult Add()
        {
            return View(new AddViewModel());
        }

        [HttpPost("add-category", Name = "admin-product-category-add")]
        public IActionResult Add(AddViewModel addViewModel)
        {
            var productCategory = new ProductCategory { Title = addViewModel.Title, CreatedAt = DateTime.Now };
            _dataContext.ProductCategories.Add(productCategory);
            _dataContext.SaveChanges();
            return RedirectToRoute("admin-product-category-list");
        }

        [HttpGet("update-category/{id}", Name = "admin-product-category-update")]
        public IActionResult Update(int id)
        {
            var productCategory = _dataContext.ProductCategories.FirstOrDefault(x => x.Id == id);
            if (productCategory is null) return NotFound();
            return View(new UpdateViewModel { Id = productCategory.Id, Title = productCategory.Title });
        }

        [HttpPost("update-category/{id}", Name = "admin-product-category-update")]
        public IActionResult Update(int id, UpdateViewModel updateViewModel)
        {
            var productCategory = _dataContext.ProductCategories.FirstOrDefault(x => x.Id == id);
            if (productCategory is null) return NotFound();

            productCategory.Title = updateViewModel.Title;

            _dataContext.SaveChanges();

            return RedirectToRoute("admin-product-category-list");
        }

        [HttpPost("delete-category/{id}", Name = "admin-product-category-delete")]
        public IActionResult Delete(int id)
        {
            var productCategory = _dataContext.ProductCategories.FirstOrDefault(x => x.Id == id);
            if (productCategory is null) return NotFound();

            _dataContext.ProductCategories.Remove(productCategory);
            _dataContext.SaveChanges();

            return RedirectToRoute("admin-product-category-list");
        }
    }
}
