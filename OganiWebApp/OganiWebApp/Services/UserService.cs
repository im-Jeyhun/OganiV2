using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database.Models;
using OganiWebApp.Database;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace OganiWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private User _currentUser;

        public UserService(
            DataContext dataContext,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated
        {
            get => _httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser is not null)
                {
                    return _currentUser;
                }

                var idClaim = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(C => C.Type == "id");
                if (idClaim is null)
                    throw new Exception("Identity cookie not found");

                _currentUser = _dataContext.Users
                    .Include(u => u.Role).First(u => u.Id == Int32.Parse(idClaim.Value));

                return _currentUser;
            }
        }

        public string GetCurrentUserFullName()
        {
            return $"{CurrentUser.FirstName} {CurrentUser.LastName}";
        }

        public async Task<bool> CheckPasswordAsync(string? email, string? password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user is not null && BC.Verify(password, user.Password);

        }

        public async Task SignInAsync(int id, string? role = null, bool rememberMe = default)
        {
            var claims = new List<Claim>
            {
                new Claim("id", id.ToString())
            };

            if (role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);

            var Remember = new AuthenticationProperties
            {
                IsPersistent = rememberMe
            };
            await _httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, Remember);
        }

        public async Task SignInAsync(string? email, string? password, string? role = null, bool rememberMe = default)
        {
            var user = await _dataContext.Users.FirstAsync(u => u.Email == email);

            if (user is not null && BC.Verify(password, user.Password))
            {
                await SignInAsync(user.Id, role, rememberMe);
            }

        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task CreateAsync(RegisterViewModel model)
        {
            var user = await CreateUserAsync();
            var basket = await CreateBasketAsync();

            await CreteBasketProductsAsync();

            await _dataContext.SaveChangesAsync();

            async Task<User> CreateUserAsync()
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = BC.HashPassword(model.Password),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,

                };
                await _dataContext.Users.AddAsync(user);

                return user;
            }

            async Task<Basket> CreateBasketAsync()
            {
                //Create basket process
                var basket = new Basket
                {
                    User = user,

                };
                await _dataContext.Baskets.AddAsync(basket);

                return basket;
            }

            async Task CreteBasketProductsAsync()
            {
               
                var productCookieValue = _httpContextAccessor.HttpContext!.Request.Cookies["products"];
                if (productCookieValue is not null)
                {
                    var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                    foreach (var productCookieViewModel in productsCookieViewModel)
                    {
                        var product = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == productCookieViewModel.Id);
                        var basketProduct = new BasketProduct
                        {
                            Basket = basket,
                            ProductId = product!.Id,
                            Quantity = productCookieViewModel.Quantity,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        await _dataContext.BasketProducts.AddAsync(basketProduct);
                    }
                }
            }
        }
       
    }
}
