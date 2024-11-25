using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database.Models;

namespace OganiWebApp.Services
{
    public interface IUserService
    {
        public bool IsAuthenticated { get; }
        public User CurrentUser { get; }

        Task<bool> CheckPasswordAsync(string? email, string? password);

        string GetCurrentUserFullName();
        Task SignInAsync(int id, string? role = null, bool rememberMe = default);
        Task SignInAsync(string? email, string? password, string? role = null, bool rememberMe = default);
        Task CreateAsync(RegisterViewModel model);
        Task SignOutAsync();

    }
}
