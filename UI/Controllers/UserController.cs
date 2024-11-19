using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UI.ApiSettings;
using UI.Dtos;
using UI.Models;
using UI.Services.Interface;

namespace UI.Controllers
{
    
    public class UserController : Controller
    {
        private readonly IClientServices<User> _clintServices;
        private readonly IClientServices<Register> _registerUserServices;
        private readonly ApiUrlSettings _apiUrls;

        public UserController(IClientServices<User> clintServices, IOptions<ApiUrlSettings> apiUrls, IClientServices<Register> registerUserServices)
        {
            _clintServices = clintServices;
            _apiUrls = apiUrls.Value;
            _registerUserServices = registerUserServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(User model)
        {
            // Initialize the Roles list with the RoleName value
            model.Roles = new List<string> { model.RoleName };
            var register = await _clintServices.PostClientAsync(_apiUrls._RegisterUrl, model);
            return Json(register);
        }
        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            var User = await _clintServices.GetAllClientsAsync(_apiUrls._UserUrl);
            return Json(User);
        }
        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {

            var Category = await _clintServices.GetClientByIdAsync($"{_apiUrls._UserUrl}/{id}");
            return Json(Category);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string id, User model)
        {
            model.Roles = new List<string> { model.RoleName };
            var result = await _clintServices.UpdateClientAsync($"{_apiUrls._UserUrl}/{id}", model);
            return Json(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _clintServices.DeleteClientAsync($"{_apiUrls._UserUrl}/{id}");
            return Json(result);
        }

        public async Task<IActionResult> CheckDuplicate(string key, string val)
        {
            var usersResponse = await _clintServices.GetAllClientsAsync(_apiUrls._UserUrl);
            if (usersResponse.Success)
            {
                bool isDuplicate = usersResponse.Data.Any(user =>
                {
                    var propertyInfo = user.GetType().GetProperty(key);
                    if (propertyInfo == null) return false;
                    var propertyValue = propertyInfo.GetValue(user, null)?.ToString();
                    return propertyValue?.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase) ?? false;
                });

                return Json(isDuplicate);
            }
            return Json(false);
        }

    }
}
