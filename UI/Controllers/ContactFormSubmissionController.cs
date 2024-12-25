using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class ContactFormSubmissionController : Controller
    {
        private readonly IClientServices<ContactFormSubmission> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public ContactFormSubmissionController(IClientServices<ContactFormSubmission> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(ContactFormSubmission model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._ContactFormSubmissionUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var ContactFormSubmissions = await _clintServices.GetAllClientsAsync(_apiUrls._ContactFormSubmissionUrl);
            return Json(ContactFormSubmissions);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var ContactFormSubmission = await _clintServices.GetClientByIdAsync($"{_apiUrls._ContactFormSubmissionUrl}/{id}");
            return Json(ContactFormSubmission);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, ContactFormSubmission model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._ContactFormSubmissionUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._ContactFormSubmissionUrl}/{id}");
            return Json(result);
        }
    }
}
