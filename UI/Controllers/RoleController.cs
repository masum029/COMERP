using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class RoleController : Controller
    {
        private readonly IClientServices<Role> _clintServices;
        private readonly ApiUrlSettings _apiUrls;
        public RoleController(IClientServices<Role> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

      

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Role = await _clintServices.GetAllClientsAsync(_apiUrls._RoleUrl);
            return Json(Role);
        }
    }
}
