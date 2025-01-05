using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IClientServices<Menu> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public MenuController(IClientServices<Menu> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Menu model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._MenuUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Menus = await _clintServices.GetAllClientsAsync(_apiUrls._MenuUrl);
            return Json(Menus);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Menu = await _clintServices.GetClientByIdAsync($"{_apiUrls._MenuUrl}/{id}");
            return Json(Menu);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Menu model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._MenuUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._MenuUrl}/{id}");
            return Json(result);
        }
    }
}
