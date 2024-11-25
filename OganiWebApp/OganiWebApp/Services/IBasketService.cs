using OganiWebApp.Areas.Client.ViewModels;
using OganiWebApp.Database.Models;

namespace OganiWebApp.Services
{
    public interface IBasketService
    {
         Task<List<ProductCookieViewModel>> AddBasketProductAsync(Product product);
    }
}
