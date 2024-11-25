using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OganiWebApp.Areas.Admin.ViewModels.Authentication;
using OganiWebApp.Database;
using OganiWebApp.Services;

namespace OganiWebApp.Areas.Admin.Controllers
{

    [Area("admin")]
    [Route("admin/auth")]
    public class AuthenticationController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IUserService _userService;

        public AuthenticationController(DataContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        #region Login and Logout

        [HttpGet("login", Name = "admin-auth-login")]
        public async Task<IActionResult> LoginAsync()
        {
            return View(new LoginViewModel());
        }

        [HttpPost("login", Name = "admin-auth-login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel? model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!await _userService.CheckPasswordAsync(model!.Email, model!.Password))
            {
                ModelState.AddModelError(String.Empty, "Email or password is not correct");
                return View(model);
            }

            if (await _dbContext.Users.AnyAsync(u => u.Email == model.Email && u.Role.Name == "admin"))
            {
                await _userService.SignInAsync(model!.Email, model!.Password, "admin");
                return RedirectToRoute("admin-user-list");
            }

            if (!await _dbContext.Users.AnyAsync(u => u.Role.Name == "admin"))
            {
                ModelState.AddModelError(String.Empty, "You are not admin");
                return View(model);
            }

            await _userService.SignInAsync(model.Email, model.Password, "admin");

            return RedirectToRoute("analyticts-index");
        }

        [HttpGet("logout", Name = "admin-auth-logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _userService.SignOutAsync();

            return RedirectToRoute("admin-auth-login");
        }

        #endregion
    }
}


