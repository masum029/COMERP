using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class CompanyDetailsController : Controller
    {
        private readonly IClientServices<CompanyDetails> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public CompanyDetailsController(IClientServices<CompanyDetails> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(CompanyDetails model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._CompanyDetailsUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var CompanyDetailss = await _clintServices.GetAllClientsAsync(_apiUrls._CompanyDetailsUrl);
            return Json(CompanyDetailss);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var CompanyDetails = await _clintServices.GetClientByIdAsync($"{_apiUrls._CompanyDetailsUrl}/{id}");
            return Json(CompanyDetails);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, CompanyDetails model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._CompanyDetailsUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._CompanyDetailsUrl}/{id}");
            return Json(result);
        }
    }
}
