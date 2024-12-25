using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class SliderController : Controller
    {
        private readonly IClientServices<Slider> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public SliderController(IClientServices<Slider> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Slider model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._SliderUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Sliders = await _clintServices.GetAllClientsAsync(_apiUrls._SliderUrl);
            return Json(Sliders);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Slider = await _clintServices.GetClientByIdAsync($"{_apiUrls._SliderUrl}/{id}");
            return Json(Slider);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Slider model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._SliderUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._SliderUrl}/{id}");
            return Json(result);
        }
    }
}
