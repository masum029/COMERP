using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientServices<Client> _clintServices;
        private readonly ApiUrlSettings _apiUrls;
        private readonly IFileUploader _fileUploader;
        public ClientController(IClientServices<Client> clintServices, IOptions<ApiUrlSettings> apiUrls, IFileUploader fileUploader)
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

        public async Task<IActionResult> Create(Client model)
        {
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                model.Icon = await _fileUploader.ImgUploader(model.FormFile[0], "Client_icom"); 
            }
            var register = await _clintServices.PostClientAsync(_apiUrls._ClientUrl, model);
            if (register.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Clients = await _clintServices.GetAllClientsAsync(_apiUrls._ClientUrl);
            return Json(Clients);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Client = await _clintServices.GetClientByIdAsync($"{_apiUrls._ClientUrl}/{id}");
            return Json(Client);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Client model)
        {
            var Client = await _clintServices.GetClientByIdAsync($"{_apiUrls._ClientUrl}/{id}");
            if (model.FormFile != null && model.FormFile.Count > 0)
            {
                if (Client?.Data?.Icon != null)
                {
                    await _fileUploader.DeleteFile(Client?.Data?.Icon, "Client_icom");

                }
                model.Icon = await _fileUploader.ImgUploader(model.FormFile[0], "Client_icom");


            }
            else
            {
                model.Icon = Client?.Data?.Icon;

            }

            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._ClientUrl}/{id}", model);
            if (result.Success)
            {
                HttpContext.Session.Remove("OldCompanyName");
            }

            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var Client = await _clintServices.GetClientByIdAsync($"{_apiUrls._ClientUrl}/{id}");
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._ClientUrl}/{id}");
            if (result.Success)
            {
                if (Client?.Data?.Icon != null)
                {
                    await _fileUploader.DeleteFile(Client?.Data?.Icon, "Client_icom");
                }

            }

            return Json(result);
        }
    }
}
