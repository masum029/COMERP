using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class PageContentController : Controller
    {
        private readonly IClientServices<PageContent> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public PageContentController(IClientServices<PageContent> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(PageContent model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._PageContentUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var PageContents = await _clintServices.GetAllClientsAsync(_apiUrls._PageContentUrl);
            return Json(PageContents);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var PageContent = await _clintServices.GetClientByIdAsync($"{_apiUrls._PageContentUrl}/{id}");
            return Json(PageContent);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, PageContent model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._PageContentUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._PageContentUrl}/{id}");
            return Json(result);
        }
    }
}
