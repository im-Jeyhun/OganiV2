using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Admin.ViewModels.Slider;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/slider")]
    //[Authorize(Roles = "admin")]
    public class SliderController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public SliderController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }
        [HttpGet("list" , Name = "admin-slider-list")]
        public IActionResult List()
        {
            var model = _dataContext.Sliders.Select(s => new ListItemViewModel
            {
                Id = s.Id,
                Title = s.Title,
                ProductCategory = _dataContext.ProductCategories.FirstOrDefault(x => x.Id == s.PorductCategoryId).Title,
                BgImageUrl = _fileService.GetFileUrl(s.BgImageNameInFileSystem, UploadDirectory.Slider)
            }).ToList();

            return View(model);
        }

        [HttpGet("add-slider", Name = "admin-slider-add")]
        public IActionResult AddAsync()
        {
            return View(new AddViewModel { ProductCategories =  _dataContext.ProductCategories.ToList() });
        }
        [HttpPost("add-slider", Name = "admin-slider-add")]
        public async Task<IActionResult> AddAsync(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var BgImageNameInFileSytstem = await _fileService.UploadAsync(model.BgImage, UploadDirectory.Slider);

            AddSlider(model.BgImage.FileName, BgImageNameInFileSytstem);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-slider-list");

            void AddSlider(string BgImageName, string BgImageNameInFileSystem)
            {
                var slider = new Slider
                {
                    Title = model.Title,
                    BgImageName = BgImageName,
                    BgImageNameInFileSystem = BgImageNameInFileSystem,
                    PorductCategoryId = model.PorductCategoryId,
                    CreatedAt = DateTime.Now
                };
                _dataContext.Add(slider);
            }
        }

        [HttpGet("slider-update/{id}", Name = "admin-slider-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            var slider = await _dataContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);

            if (slider is null)
            {
                return NotFound();
            }

            var model = new UpdateViewModel
            {
                Id = slider.Id,
                Title = slider.Title,
                BackgroundImageUrl = _fileService.GetFileUrl(slider.BgImageNameInFileSystem, UploadDirectory.Slider),
                PorductCategoryId = slider.PorductCategoryId,
                ProductCategories =  _dataContext.ProductCategories.ToList(),
            };

            return View(model);
        }
        [HttpPost("slider-update/{id}", Name = "admin-slider-update")]
        public async Task<IActionResult> UpdateAsync(UpdateViewModel model , int id)
        {
            var slider = await _dataContext.Sliders.FirstOrDefaultAsync(s => s.Id == model.Id);
            if (slider is null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.BackgroundImageUrl = _fileService.GetFileUrl(slider.BgImageNameInFileSystem, UploadDirectory.Slider);
                return View(model);
            }

            if (model.BgImage is not null)
            {
                await _fileService.DeleteAsync(slider.BgImageNameInFileSystem, UploadDirectory.Slider);
                var imageFileNameInSystem = await _fileService.UploadAsync(model.BgImage, UploadDirectory.Slider);
                slider.BgImageName = slider.BgImageName;
                slider.BgImageNameInFileSystem = imageFileNameInSystem;

            }
            else
            {
                slider.Title = model.Title;
                slider.PorductCategoryId = model.PorductCategoryId;
            }

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-slider-list");
        }

        [HttpPost("slider-delete/{id}", Name = "admin-slider-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var slider = await _dataContext.Sliders.FirstOrDefaultAsync(b => b.Id == id);
            if (slider is null)
            {
                return NotFound();
            }

            await _fileService.DeleteAsync(slider.BgImageNameInFileSystem, UploadDirectory.Slider);

            _dataContext.Sliders.Remove(slider);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-slider-list");
        }
    }
}
