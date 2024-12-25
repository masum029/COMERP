using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IClientServices<Service> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public ServiceController(IClientServices<Service> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Service model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._ServiceUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Services = await _clintServices.GetAllClientsAsync(_apiUrls._ServiceUrl);
            return Json(Services);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Service = await _clintServices.GetClientByIdAsync($"{_apiUrls._ServiceUrl}/{id}");
            return Json(Service);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Service model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._ServiceUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._ServiceUrl}/{id}");
            return Json(result);
        }
    }
}
