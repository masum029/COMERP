using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionServices _session;

        public HomeController(ILogger<HomeController> logger, ISessionServices session)
        {
            _logger = logger;
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            var company = await _session.GetCompanyFromSession();
            // Pass the company object to the layout using ViewData or ViewBag
            ViewData["Company"] = company;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
