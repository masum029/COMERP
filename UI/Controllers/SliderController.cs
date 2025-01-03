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
        private readonly IFileUploader _fileUploader;

        public SliderController(IClientServices<Slider> clintServices, IOptions<ApiUrlSettings> apiUrls, IFileUploader fileUploader)
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

        public async Task<IActionResult> Create(Slider model)
        {
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                model.ImageUrl = await _fileUploader.ImgUploader(model.FormFile[0], "Slider");
            }

            var register = await _clintServices.PostClientAsync(_apiUrls._SliderUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

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
            var Slider = await _clintServices.GetClientByIdAsync($"{_apiUrls._SliderUrl}/{id}");
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                if (Slider?.Data?.ImageUrl != null)
                {
                    await _fileUploader.DeleteFile(Slider?.Data?.ImageUrl, "Slider");

                }
                model.ImageUrl = await _fileUploader.ImgUploader(model.FormFile[0], "Slider");
            }
            else
            {
                model.ImageUrl = Slider?.Data?.ImageUrl;

            }

            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._SliderUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var Slider = await _clintServices.GetClientByIdAsync($"{_apiUrls._SliderUrl}/{id}");

            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._SliderUrl}/{id}");
            if (result.Success)
            {
                if (Slider?.Data?.ImageUrl != null)
                {
                    await _fileUploader.DeleteFile(Slider?.Data?.ImageUrl, "Slider");
                }

            }

            return Json(result);
        }
    }
}
