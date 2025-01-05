using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;
using UI.ViewModel;

namespace UI.Controllers
{
    public class AboutApiController : Controller
    {
        private readonly IClientServices<About> _clintServices;
        private readonly ApiUrlSettings _apiUrls;
        private readonly IFileUploader _fileUploader;
        public AboutApiController(IClientServices<About> clintServices, IOptions<ApiUrlSettings> apiUrls, IFileUploader fileUploader)
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
        public async Task<IActionResult> Create(AboutVm model)
        {
            // Upload the main About image
            if (model.About.FormFile != null && model.About.FormFile.Count > 0)
            {
                model.About.Img = await _fileUploader.ImgUploader(model.About.FormFile[0], "About");
            }

            // Process SubAbouts
            if (model.subAbouts != null)
            {
                // Initialize the SubAbouts collection in About
                model.About.SubAbouts = model.subAbouts.ToList();

                foreach (var subAbout in model.About.SubAbouts)
                {
                    subAbout.AboutId = model.About.Id; // Link SubAbouts to About

                    // Find and associate AboutTopics for the current SubAbout
                    if (model.aboutTopics != null)
                    {
                        subAbout.AboutTopics = model.aboutTopics
                            .Where(at => at.SubAboutId == subAbout.Id) // Use TempId to associate AboutTopics
                            .Select(at =>
                            {
                                at.SubAboutId = subAbout.Id; // Link AboutTopic to SubAbout
                                return at;
                            })
                            .ToList();
                    }
                }
            }

            // Save the About entity to the database
            var register = await _clintServices.PostClientAsync(_apiUrls._AboutUrl, model.About);

            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
                return Json(register);
            }

            return Json(new { Success = false, Message = "Invalid data" });
        }


        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Companys = await _clintServices.GetAllClientsAsync(_apiUrls._AboutUrl);
            return Json(Companys);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Company = await _clintServices.GetClientByIdAsync($"{_apiUrls._AboutUrl}/{id}");
            return Json(Company);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, AboutVm model)
        {
            var about = await _clintServices.GetClientByIdAsync($"{_apiUrls._AboutUrl}/{id}");
            if (model.About.FormFile != null && model.About.FormFile.Count > 0)
            {
                if (about?.Data?.Img != null)
                {
                    await _fileUploader.DeleteFile(about?.Data?.Img, "About");

                }
                model.About.Img = await _fileUploader.ImgUploader(model.About.FormFile[0], "About");


            }
            else
            {
                model.About.Img = about?.Data?.Img;

            }

            // Process SubAbouts
            if (model.subAbouts != null)
            {
                // Initialize the SubAbouts collection in About
                model.About.SubAbouts = model.subAbouts.ToList();

                foreach (var subAbout in model.About.SubAbouts)
                {
                    subAbout.AboutId = model.About.Id; // Link SubAbouts to About

                    // Find and associate AboutTopics for the current SubAbout
                    if (model.aboutTopics != null)
                    {
                        subAbout.AboutTopics = model.aboutTopics
                            .Where(at => at.SubAboutId == subAbout.Id) // Use TempId to associate AboutTopics
                            .Select(at =>
                            {
                                at.SubAboutId = subAbout.Id; // Link AboutTopic to SubAbout
                                return at;
                            })
                            .ToList();
                    }
                }
            }



            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._AboutUrl}/{id}", model.About);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var Company = await _clintServices.GetClientByIdAsync($"{_apiUrls._AboutUrl}/{id}");
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._AboutUrl}/{id}");
            if (result.Success)
            {
                if (Company?.Data?.Img != null)
                {
                    await _fileUploader.DeleteFile(Company?.Data?.Img, "About");
                }

            }
            return Json(result);
        }
    }
}
