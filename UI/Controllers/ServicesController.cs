using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
