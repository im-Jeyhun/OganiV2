using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Database.Models;
using OganiWebApp.Database;
using OganiWebApp.Areas.Admin.ViewModels.Size;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/size")]
    //[Authorize(Roles = "admin")]
    public class SizeController : Controller
    {
        private readonly DataContext _dataContext;

        public SizeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("list", Name = "admin-size-list")]
        public IActionResult List()
        {
            var productsizeVM = _dataContext.Sizes.Select(x => new ListItemViewModel(x.Id, x.Title)).ToList();
            return View(productsizeVM);
        }
        [HttpGet("add-size", Name = "admin-size-add")]
        public IActionResult Add()
        {
            return View(new AddViewModel());
        }

        [HttpPost("add-size", Name = "admin-size-add")]
        public IActionResult Add(AddViewModel addViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(addViewModel);
            }
            var productsize = new Size { Title = addViewModel.Title, CreatedAt = DateTime.Now , UpdatedAt = DateTime.Now };
            _dataContext.Sizes.Add(productsize);
            _dataContext.SaveChanges();
            return RedirectToRoute("admin-size-list");
        }

        [HttpGet("update-size/{id}", Name = "admin-size-update")]
        public IActionResult Update(int id)
        {
            var productsize = _dataContext.Sizes.FirstOrDefault(x => x.Id == id);
            if (productsize is null) return NotFound();
            return View(new UpdateViewModel { Id = productsize.Id, Title = productsize.Title });
        }

        [HttpPost("update-size/{id}", Name = "admin-size-update")]
        public IActionResult Update(int id, UpdateViewModel updateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateViewModel);
            }
            var productsize = _dataContext.Sizes.FirstOrDefault(x => x.Id == id);
            if (productsize is null) return NotFound();

            productsize.Title = updateViewModel.Title;
            productsize.UpdatedAt = DateTime.Now;

            _dataContext.SaveChanges();

            return RedirectToRoute("admin-size-list");
        }

        [HttpPost("delete-size/{id}", Name = "admin-size-delete")]
        public IActionResult Delete(int id, UpdateViewModel updateViewModel)
        {
            var productsize = _dataContext.Sizes.FirstOrDefault(x => x.Id == id);
            if (productsize is null) return NotFound();

            _dataContext.Sizes.Remove(productsize);
            _dataContext.SaveChanges();

            return RedirectToRoute("admin-size-list");
        }
    }
}
