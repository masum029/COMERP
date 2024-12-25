using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class EventController : Controller
    {
        private readonly IClientServices<Event> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public EventController(IClientServices<Event> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Event model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._EventUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Events = await _clintServices.GetAllClientsAsync(_apiUrls._EventUrl);
            return Json(Events);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Event = await _clintServices.GetClientByIdAsync($"{_apiUrls._EventUrl}/{id}");
            return Json(Event);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Event model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._EventUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._EventUrl}/{id}");
            return Json(result);
        }
    }
}
