using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IClientServices<Company> _clintServices;
        private readonly ApiUrlSettings _apiUrls;
        private readonly IFileUploader _fileUploader;
        public CompanyController(IClientServices<Company> clintServices, IOptions<ApiUrlSettings> apiUrls, IFileUploader fileUploader)
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

        public async Task<IActionResult> Create(Company model)
        {
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                model.Logo = await _fileUploader.ImgUploader(model.FormFile[0], "Logo");
                if (model.isActive) await _fileUploader.SetFavicon(model.FormFile[0]);
            }
            var register = await _clintServices.PostClientAsync(_apiUrls._CompanyUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Companys = await _clintServices.GetAllClientsAsync(_apiUrls._CompanyUrl);
            return Json(Companys);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Company = await _clintServices.GetClientByIdAsync($"{_apiUrls._CompanyUrl}/{id}");
            return Json(Company);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Company model)
        {
            var Company = await _clintServices.GetClientByIdAsync($"{_apiUrls._CompanyUrl}/{id}");
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                if (Company?.Data?.Logo != null)
                {
                    await _fileUploader.DeleteFile(Company?.Data?.Logo, "Logo");

                }
                model.Logo = await _fileUploader.ImgUploader(model.FormFile[0], "Logo");
                if(model.isActive) await _fileUploader.SetFavicon(model.FormFile[0]);

            }
            else
            {
                model.Logo = Company?.Data?.Logo;
                
            }
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._CompanyUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var Company = await _clintServices.GetClientByIdAsync($"{_apiUrls._CompanyUrl}/{id}");
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._CompanyUrl}/{id}");
            if (result.Success)
            {
                if (Company?.Data?.Logo != null)
                {
                    await _fileUploader.DeleteFile(Company?.Data?.Logo, "Logo");
                }

            }
            return Json(result);
        }

    }
}
