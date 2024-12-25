using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class SiteSettingsController : Controller
    {
        private readonly IClientServices<SiteSettings> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public SiteSettingsController(IClientServices<SiteSettings> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(SiteSettings model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._SiteSettingsUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var SiteSettingss = await _clintServices.GetAllClientsAsync(_apiUrls._SiteSettingsUrl);
            return Json(SiteSettingss);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var SiteSettings = await _clintServices.GetClientByIdAsync($"{_apiUrls._SiteSettingsUrl}/{id}");
            return Json(SiteSettings);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, SiteSettings model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._SiteSettingsUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._SiteSettingsUrl}/{id}");
            return Json(result);
        }
    }
}
