using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class PortfolioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
