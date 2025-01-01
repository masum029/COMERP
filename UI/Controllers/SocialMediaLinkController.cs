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
        private readonly IFileUploader _fileUploader;

        public SocialMediaLinkController(IClientServices<SocialMediaLink> clintServices, IOptions<ApiUrlSettings> apiUrls, IFileUploader fileUploader)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
            _fileUploader = fileUploader;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(SocialMediaLink model)
        {
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                model.IconUrl = await _fileUploader.ImgUploader(model.FormFile[0], "Social_Icon");
            }
            var register = await _clintServices.PostClientAsync(_apiUrls._SocialMediaLinkUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
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
            var SocialMediaLink = await _clintServices.GetClientByIdAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}");
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                if (SocialMediaLink?.Data?.IconUrl != null)
                {
                    await _fileUploader.DeleteFile(SocialMediaLink?.Data?.IconUrl, "Social_Icon");

                }
                model.IconUrl = await _fileUploader.ImgUploader(model.FormFile[0], "Social_Icon");
                

            }
            else
            {
                model.IconUrl = SocialMediaLink?.Data?.IconUrl;

            }
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var SocialMediaLink = await _clintServices.GetClientByIdAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}");
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._SocialMediaLinkUrl}/{id}");
            if (result.Success)
            {
                if (SocialMediaLink?.Data?.IconUrl != null)
                {
                    await _fileUploader.DeleteFile(SocialMediaLink?.Data?.IconUrl, "Social_Icon");
                }

            }
            
            return Json(result);
        }
    }
}
