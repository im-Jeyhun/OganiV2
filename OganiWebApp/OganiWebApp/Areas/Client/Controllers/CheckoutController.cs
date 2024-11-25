using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Database.Models;
using OganiWebApp.Database;
using OganiWebApp.Services;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Contracts;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("checkout")]
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserService _userService;
        public CheckoutController(DataContext dataContext, IUserService userService)
        {
            _dataContext = dataContext;
            _userService = userService;
        }
        [HttpGet("checkoutlist", Name = "client-checkout-checkoutlist")]
        public async Task<IActionResult> Checkoutlist()
        {
            var model = new ProductListItemViewModel
            {

                Products = await _dataContext.BasketProducts.Include(bp => bp.Product)
                 .Select(bp => new ProductListItemViewModel.ListItem(bp.ProductId, bp.Product.Name, (int)bp.Quantity, bp.Product.Price, (decimal)(bp.Product.Price * bp.Quantity)))
                 .ToListAsync()
            };
            return View(model);
        }

        [HttpPost("place-order", Name = "client-checkout-place-order")]
        public async Task<IActionResult> PlaceOrder()
        {
            var pasketProducts = _dataContext.BasketProducts.Include(bp => bp.Product).Select(bp => new
            ProductListItemViewModel.ListItem(bp.ProductId, bp.Product.Name, (int)bp.Quantity, bp.Product.Price, (decimal)(bp.Product.Price * bp.Quantity))).ToList();

            var createOrder = await CreateOrder();

            foreach (var basketProduct in pasketProducts)
            {
                var orderProduct = new OrderProduct
                {
                    ProductId = basketProduct.Id,
                    Quantity = basketProduct.Quantity,
                    OrderId = createOrder.Id,
                    Order = createOrder
                };

                _dataContext.OrderProducts.Add(orderProduct);
            }

            await DeleteBasketProducts();

            _dataContext.SaveChanges();

            async Task<Order> CreateOrder()
            {
                var order = new Order
                {
                    UserId = _userService.CurrentUser.Id,
                    Status = (int)Status.Created,
                    SumTotalPrice = (decimal)_dataContext.BasketProducts.
                    Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).Sum(bp => bp.Product.Price * bp.Quantity)

                };

                await _dataContext.Orders.AddAsync(order);

                return order;

            }

            async Task DeleteBasketProducts()
            {
                var removedBasketProducts = await _dataContext.BasketProducts
                       .Where(bp => bp.Basket.UserId == _userService.CurrentUser.Id).ToListAsync();

                removedBasketProducts.ForEach(bp => _dataContext.BasketProducts.Remove(bp));
            }

            return RedirectToRoute("client-checkout-checkoutlist");
        }

    }
}
