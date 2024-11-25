using Microsoft.AspNetCore.Mvc;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database.Models;
using OganiWebApp.Database;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IUserService _userService;

        public AuthenticationController(DataContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet("login", Name = "client-auth-login")]
        public async Task<IActionResult> Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("login", Name = "client-auth-login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userService.SignInAsync(model!.Email, model!.Password, "user");

            return RedirectToRoute("client-home-index");
        }

        [HttpGet("register", Name = "client-auth-register")]
        public async Task<IActionResult> Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("register", Name = "client-auth-register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            await _userService.CreateAsync(registerViewModel);
            return RedirectToRoute("client-auth-login");

        }
        [HttpGet("logout", Name = "client-auth-logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _userService.SignOutAsync();

            return RedirectToRoute("client-home-index");
        }
    }
}
