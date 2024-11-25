using Microsoft.AspNetCore.Mvc;

namespace OganiWebApp.Areas.Client.Controllers
{
    [Area("client")]
    [Route("contact")]
    public class ContactController : Controller
    {
        [HttpGet("index", Name = "client-contact-index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
