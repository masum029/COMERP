using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class PricingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
