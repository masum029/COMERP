using Microsoft.AspNetCore.Mvc;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class PricingController : Controller
    {
        private readonly ISessionServices _session;

        public PricingController(ISessionServices session)
        {
            _session = session;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Company"] = await _session.GetCompanyFromSession();
            return View();
        }
    }
}
