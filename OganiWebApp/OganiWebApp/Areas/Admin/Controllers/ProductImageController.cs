using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Admin.ViewModels.Product;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Database.Models;
using OganiWebApp.Services;
using System.Linq;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/product-image")]
    //[Authorize(Roles = "admin")]
    public class ProductImageController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IFileService _fileService;
        public ProductImageController(DataContext dataContext, IFileService fileService)
        {
            _dataContext = dataContext;
            _fileService = fileService;
        }

        [HttpGet("{productId}/image/list", Name = "admin-product-image-list")]
        public async Task<IActionResult> ListAsync(int productId)
        {
            var product = await _dataContext.Products.Include(b => b.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            var model = new ProductImageListItemViewModel
            {
                ProductId = product.Id,
                ProductImages = _dataContext.ProductImages.Where(x => x.ProductId == productId).Select(x => new ProductImageList
                {
                    ProductImageId = x.Id,
                    ImageUrl = _fileService.GetFileUrl(x.ImageNameInFileSystem, UploadDirectory.Proudct)

                }).ToList()
            };

            return View(model);
        }

        [HttpGet("{productId}/image/add", Name = "admin-product-image-add")]
        public async Task<IActionResult> AddAsync(int productId)
        {
            return View(new ProductImageAddViewModel { ProductId = productId});
        }

        [HttpPost("{productId}/image/add", Name = "admin-product-image-add")]
        public async Task<IActionResult> AddAsync([FromRoute] int productId, ProductImageAddViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var product = await _dataContext.Products.Include(b => b.ProductImages)
               .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();

            var productImageNameInFileSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Proudct);

            AddProductImage(model.Image.FileName, productImageNameInFileSystem);

            await _dataContext.SaveChangesAsync();


            void AddProductImage(string ImageName, string ImageNameInFileSystem)
            {
                var productImage = new ProductImage
                {
                    ImageName = ImageName,
                    ImageNameInFileSystem = ImageNameInFileSystem,
                    Product = product,
                    Order = model.Order
                };

                _dataContext.ProductImages.AddAsync(productImage);

            }

            return RedirectToRoute("admin-product-image-list", new { productId = product.Id });
        }

        [HttpGet("{productId}/image/{productImageId}/update", Name = "admin-product-image-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int productId, int productImageId)
        {
            var productImage = await _dataContext.ProductImages
                .FirstOrDefaultAsync(bi => bi.Id == productImageId && bi.ProductId == productId);

            if (productImage is null) return NotFound();

            var model = new ProductImageUpdateViewModel
            {
                ProductId = productImage.ProductId,
                ProductImgId = productImage.Id,
                ImageUrl = _fileService.GetFileUrl(productImage.ImageNameInFileSystem, UploadDirectory.Proudct),

            };

            return View(model);
        }

        [HttpPost("{productId}/image/{productImageId}/update", Name = "admin-product-image-update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int productId, int productImageId, ProductImageUpdateViewModel model)
        {
            var productImage = await _dataContext.ProductImages
                  .FirstOrDefaultAsync(bi => bi.Id == productImageId && bi.ProductId == productId);

            if (productImage is null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.ImageUrl = _fileService.GetFileUrl(productImage.ImageNameInFileSystem, UploadDirectory.Proudct);
                return View(model);
            }


            if (model.Image is not null)
            {
                await _fileService.DeleteAsync(productImage.ImageNameInFileSystem, UploadDirectory.Proudct);
                var productImageFileNameInSystem = await _fileService.UploadAsync(model.Image, UploadDirectory.Proudct);
                await UpdateProductImageAsync(model.Image.FileName, productImageFileNameInSystem);
            }

            UpdateProductImageOrder();

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-product-image-list", new { productId = productImage.ProductId });

            async Task UpdateProductImageAsync(string productImageName, string productImageFileNameInSystem)
            {
                productImage.ImageName = productImageName;
                productImage.ImageNameInFileSystem = productImageFileNameInSystem;
            };

            void UpdateProductImageOrder()
            {
                productImage.Order = model.Order;
            }

        }



        [HttpPost("{productId}/image/{productImageId}/delete", Name = "admin-product-image-delete")]
        public async Task<IActionResult> DeleteAsync(int productId, int productImageId)
        {
            var productImage = await _dataContext.ProductImages
                 .FirstOrDefaultAsync(bi => bi.Id == productImageId && bi.ProductId == productId);

            if (productImage is null) return NotFound();

            await _fileService.DeleteAsync(productImage.ImageNameInFileSystem, UploadDirectory.Proudct);

            _dataContext.ProductImages.Remove(productImage);

            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("admin-product-image-list", new { productId = productImage.ProductId });
        }



    }
}
