using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IClientServices<Project> _clintServices;
        private readonly ApiUrlSettings _apiUrls;

        public ProjectController(IClientServices<Project> clintServices, IOptions<ApiUrlSettings> apiUrls)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(Project model)
        {
            var register = await _clintServices.PostClientAsync(_apiUrls._ProjectUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var Projects = await _clintServices.GetAllClientsAsync(_apiUrls._ProjectUrl);
            return Json(Projects);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Project = await _clintServices.GetClientByIdAsync($"{_apiUrls._ProjectUrl}/{id}");
            return Json(Project);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, Project model)
        {
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._ProjectUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._ProjectUrl}/{id}");
            return Json(result);
        }
    }
}
