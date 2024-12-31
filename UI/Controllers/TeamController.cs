using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
