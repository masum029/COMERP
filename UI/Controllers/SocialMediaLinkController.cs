using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class SocialMediaLinkController : Controller
    {
        private readonly IClientServices<SocialMediaLink> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public SocialMediaLinkController(IClientServices<SocialMediaLink> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(SocialMediaLink model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._SocialMediaLinkUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var SocialMediaLinks = await _clintServices.GetAllClientsAsync(_apiUrls._SocialMediaLinkUrl);
            return Json(SocialMediaLinks);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var SocialMediaLink = await _clintServices.GetClientByIdAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}");
            return Json(SocialMediaLink);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, SocialMediaLink model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}");
            return Json(result);
        }
    }
}
