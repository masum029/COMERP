using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IClientServices<Company> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public CompanyController(IClientServices<Company> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Company model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._CompanyUrl, model);
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
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._CompanyUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._CompanyUrl}/{id}");
            return Json(result);
        }

    }
}
