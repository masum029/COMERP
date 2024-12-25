using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class NewsController : Controller
    {
        private readonly IClientServices<News> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public NewsController(IClientServices<News> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(News model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._NewsUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Newss = await _clintServices.GetAllClientsAsync(_apiUrls._NewsUrl);
            return Json(Newss);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var News = await _clintServices.GetClientByIdAsync($"{_apiUrls._NewsUrl}/{id}");
            return Json(News);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, News model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._NewsUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._NewsUrl}/{id}");
            return Json(result);
        }
    }
}
