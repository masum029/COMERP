using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientServices<Client> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public ClientController(IClientServices<Client> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Client model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._ClientUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Clients = await _clintServices.GetAllClientsAsync(_apiUrls._ClientUrl);
            return Json(Clients);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Client = await _clintServices.GetClientByIdAsync($"{_apiUrls._ClientUrl}/{id}");
            return Json(Client);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Client model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._ClientUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._ClientUrl}/{id}");
            return Json(result);
        }
    }
}
