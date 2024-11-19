using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
