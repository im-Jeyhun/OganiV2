using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Admin.ViewModels.Product;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;
using OganiWebApp.Services;
using static System.Net.Mime.MediaTypeNames;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/product")]
    //[Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;

        public ProductController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("list", Name = "admin-product-list")]
        public IActionResult List()
        {
            var productViewModel = _dataContext.Products.Select(x => new ListItemViewModel(x.Id, x.Name, _fileService.GetFileUrl(x.MainImageNameInFileSystem, Contracts.UploadDirectory.Proudct))).ToList();
            return View(productViewModel);
        }

        [HttpGet("add", Name = "admin-product-add")]
        public async Task<IActionResult> AddAsync()
        {
            var categories = await _dataContext.ProductCategories.ToListAsync();
            var sizes = await _dataContext.Sizes.ToListAsync();

            var addViewModel = new AddViewModel { ProductCategories = categories, ProductSizes = sizes };
            return View(addViewModel);
        }

        [HttpPost("add", Name = "admin-product-add")]
        public async Task<IActionResult> AddAsync(AddViewModel addViewModel)
        {
            var categories = await _dataContext.ProductCategories.ToListAsync();
            var sizes = await _dataContext.Sizes.ToListAsync();
            if (!ModelState.IsValid)
            {
                addViewModel.ProductCategories = categories;
                addViewModel.ProductSizes = sizes;
                return View(addViewModel);
            }
            if (!CheckAllModel(addViewModel.SizeIds, addViewModel.PorductCategoryId, ModelState))
            {
                addViewModel.ProductCategories = categories;
                addViewModel.ProductSizes = sizes;
                return View(addViewModel);
            }

            var product = new Product
            {
                Name = addViewModel.Name,
                Shipping = addViewModel.Shipping,
                Weight = addViewModel.Weight,
                Description = addViewModel.Description,
                RemindCount = addViewModel.RemindCount,
                Price = addViewModel.Price,
                DiscountPrice = addViewModel.DiscountPrice,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                PorductCategoryId = addViewModel.PorductCategoryId,
                MainImageNameInFileSystem = addViewModel.MainImage != null
                                        ? await _fileService.UploadAsync(addViewModel.MainImage, Contracts.UploadDirectory.Proudct)
                                        : default,
                MainImageName = addViewModel.MainImage.FileName
            };
            foreach (var sizeId in addViewModel.SizeIds)
            {
                var productSize = new ProductSize
                {
                    SizeId = sizeId,
                    Product = product,
                };
                _dataContext.ProductSizes.Add(productSize);
            }

            if (addViewModel.Images != null)
            {
                foreach (IFormFile image in addViewModel.Images)
                {
                    var imageNameInSystem = await _fileService.UploadAsync(image, UploadDirectory.Proudct);
                    var productImage = new ProductImage
                    {
                        ImageName = image.FileName,
                        ImageNameInFileSystem = imageNameInSystem,
                        Product = product
                    };
                    await _dataContext.ProductImages.AddAsync(productImage);
                }
            }
            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-product-list");
        }

        [HttpGet("update/{id}", Name = "admin-product-update")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var product = await _dataContext.Products
               .Include(p => p.ProductSizes)
               .FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return NotFound();
            var categories = await _dataContext.ProductCategories.ToListAsync();
            var sizes = await _dataContext.Sizes.ToListAsync();
            var productUpdateViewModel = new UpdateViewModel
            {
                Id = id,
                Name = product.Name,
                Shipping = product.Shipping,
                Weight = product.Weight,
                Description = product.Description,
                RemindCount = product.RemindCount,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                PorductCategoryId = product.PorductCategoryId,
                MainImageUrl = _fileService.GetFileUrl(product.MainImageNameInFileSystem, UploadDirectory.Proudct),
                SizeIds = product.ProductSizes.Select(x => x.Id).ToList(),
                ProductCategories = await _dataContext.ProductCategories.ToListAsync(),
                ProductSizes = await _dataContext.Sizes.ToListAsync(),
            };

            return View(productUpdateViewModel);
        }

        [HttpPost("update/{id}", Name = "admin-product-update")]
        public async Task<IActionResult> UpdateAsync(UpdateViewModel updateViewModel, int id)
        {
            var categories = await _dataContext.ProductCategories.ToListAsync();
            var sizes = await _dataContext.Sizes.ToListAsync();
            if (!ModelState.IsValid)
            {
                updateViewModel.ProductCategories = categories;
                updateViewModel.ProductSizes = sizes;
                return View(updateViewModel);
            }
            if (updateViewModel.SizeIds != null)
            {

                if (!CheckAllModel(updateViewModel.SizeIds, updateViewModel.PorductCategoryId, ModelState))
                {
                    updateViewModel.ProductCategories = categories;
                    updateViewModel.ProductSizes = sizes;
                    return View(updateViewModel);
                }
            }

            var product = _dataContext.Products
               .Include(p => p.ProductSizes)
               .FirstOrDefault(p => p.Id == id);

            if (product is null) return NotFound();

            product.Name = updateViewModel.Name;
            product.Description = updateViewModel.Description;
            product.Price = updateViewModel.Price;
            product.DiscountPrice = updateViewModel.DiscountPrice;
            product.Weight = updateViewModel.Weight;
            product.Shipping = updateViewModel.Shipping;
            product.PorductCategoryId = updateViewModel.PorductCategoryId;
            product.RemindCount = updateViewModel.RemindCount;
            product.UpdatedAt = DateTime.Now;

            if (updateViewModel.SizeIds != null)
            {
                UpdateProductSize(updateViewModel, product);
            }
            if (updateViewModel.MainImage != null)
            {
                await _fileService.DeleteAsync(product!.MainImageNameInFileSystem!, UploadDirectory.Proudct);
                var mainImageNameInFileSystem = await _fileService.UploadAsync(updateViewModel.MainImage, UploadDirectory.Proudct);
                product.MainImageNameInFileSystem = mainImageNameInFileSystem;
                product.MainImageName = updateViewModel.MainImage.FileName;
            }

            await _dataContext.SaveChangesAsync();
            return RedirectToRoute("admin-product-list");

        }

        [HttpPost("delete/{id}", Name = "admin-product-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var product = await _dataContext.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            await _fileService.DeleteAsync(product.MainImageNameInFileSystem, UploadDirectory.Proudct);

            return RedirectToRoute("admin-product-list");

        }


        public bool CheckAllModel(List<int> SizeIds, int CategoryId, ModelStateDictionary ModelState)
        {
            var isSizesExists = CheckSizes(SizeIds, ModelState);

            var isCategoryExists = CheckCategory(CategoryId, ModelState);

            if (isSizesExists || isCategoryExists)
            {
                return true;
            }

            return false;
        }

        private bool CheckSizes(List<int> SizeIds, ModelStateDictionary ModelState)
        {
            ArgumentNullException.ThrowIfNull(SizeIds);

            var sizeExists = Enumerable.SequenceEqual(_dataContext.Sizes.Select(s => s.Id).ToList(), SizeIds);

            foreach (var sizeId in SizeIds)
            {
                if (!_dataContext.Sizes.Any(c => c.Id == sizeId))
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong");

                    return false;
                }

            }
            return true;
        }

        private bool CheckCategory(int CategoryId, ModelStateDictionary ModelState)
        {
            if (!_dataContext.ProductCategories.Any(c => c.Id == CategoryId))
            {
                ModelState.AddModelError(string.Empty, "Something went wrong");

                return false;
            }

            return true;
        }

        private void UpdateProductSize(UpdateViewModel model, Product product)
        {
            //databazada olan productin olculerini getirmek
            var sizesInDb = product!.ProductSizes.Select(p => p.SizeId).ToList();

            //databazada producta aid olan olculeri ayirmaq modeldeki olculerle
            var sizesToRemove = sizesInDb.Except(model.SizeIds).ToList();

            //modelden gelen yeni olculer databasada producta aid olan olculerden ayirmaq
            var sizesToAdd = model.SizeIds.Except(sizesInDb).ToList();

            product.ProductSizes.RemoveAll(ps => sizesToRemove.Contains(ps.SizeId));

            foreach (var sizeId in sizesToAdd)
            {
                var productSize = new ProductSize { Product = product, SizeId = sizeId };

                _dataContext.ProductSizes.Add(productSize);
            }
        }
    }
}
