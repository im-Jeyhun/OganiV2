using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Contracts;
using OganiWebApp.Database;
using OganiWebApp.Services;
using System.Text.Json;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public CartController(DataContext dataContext, IUserService userService, IFileService fileService)
        {
            _dataContext = dataContext;
            _userService = userService;
            _fileService = fileService;
        }
        [HttpGet("cart-list", Name = "client-cart-list")]
        public async Task<IActionResult> ListAsync()
        {
            if (_userService.IsAuthenticated)
            {
                var model = await _dataContext.BasketProducts.Include(bp => bp.Product)
                    .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id)
                    .Select(bp =>
                        new ProductCookieViewModel(
                            bp.ProductId,
                            bp.Product!.Name,
                            _fileService.GetFileUrl(bp.Product.MainImageNameInFileSystem , UploadDirectory.Proudct),
                            bp.Quantity,
                            bp.Product.Price,
                            bp.Product.DiscountPrice,
                            bp.Product.Price * bp.Quantity))
                    .ToListAsync();

                return View(model);
            }


            var productsCookieValue = HttpContext.Request.Cookies["products"];
            var productsCookieViewModel = new List<ProductCookieViewModel>();
            if (productsCookieValue is not null)
            {
                productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productsCookieValue);
            }



            return View(productsCookieViewModel);
        }

        [HttpGet("cart-delete/{ProductId}", Name = "client-cart-delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int productId)
        {
            if (!_userService.IsAuthenticated)
            {

                var product = await _dataContext.Products.FirstOrDefaultAsync(b => b.Id == productId);
                if (product is null)
                {
                    return NotFound();
                }

                var productCookieValue = HttpContext.Request.Cookies["products"];
                if (productCookieValue is null)
                {
                    return NotFound();
                }

                var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == productId);

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));
            }
            else
            {

                var basketProduct = await _dataContext.BasketProducts
                        .FirstOrDefaultAsync(bp => bp.Basket.UserId == _userService.CurrentUser.Id && bp.ProductId == productId);

                if (basketProduct is null) return NotFound();

                _dataContext.BasketProducts.Remove(basketProduct);
            }


            await _dataContext.SaveChangesAsync();

            return RedirectToRoute("client-cart-list");
        }

    }
}
