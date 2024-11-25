using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OganiWebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/dashboard")]
    //[Authorize("admin")]
    public class DashboardController : Controller
    {
        [HttpGet("index", Name = "dashboard-index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
