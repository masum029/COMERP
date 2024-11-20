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
        [HttpPost]

        public async Task<IActionResult> Create(Role model)
        {
            var result = await _clintServices.PostClientAsync(_apiUrls._RoleUrl, model);
            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Roles = await _clintServices.GetAllClientsAsync(_apiUrls._RoleUrl);
            return Json(Roles);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Role = await _clintServices.GetClientByIdAsync($"{_apiUrls._RoleUrl}/{id}");
            return Json(Role);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Role model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._RoleUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._RoleUrl}/{id}");
            return Json(result);
        }
    }
}
