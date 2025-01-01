using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class DashboirdController : Controller
    {
        private readonly ISessionServices _session;

        public DashboirdController(ISessionServices session)
        {
            _session = session;
        }

        public IActionResult Index()
        {
            var company = _session.GetCompanyFromSession();
            ViewData["Company"] = company;
            return View();
        }
    }
}
