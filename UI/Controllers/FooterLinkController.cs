using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class FooterLinkController : Controller
    {
        private readonly IClientServices<FooterLink> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public FooterLinkController(IClientServices<FooterLink> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(FooterLink model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._FooterLinkUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var FooterLinks = await _clintServices.GetAllClientsAsync(_apiUrls._FooterLinkUrl);
            return Json(FooterLinks);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var FooterLink = await _clintServices.GetClientByIdAsync($"{_apiUrls._FooterLinkUrl}/{id}");
            return Json(FooterLink);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, FooterLink model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._FooterLinkUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._FooterLinkUrl}/{id}");
            return Json(result);
        }
    }
}
