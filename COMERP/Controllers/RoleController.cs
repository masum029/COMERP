
using COMERP.Abstractions.Interfaces;
using COMERP.Common;
using COMERP.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace COMERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RoleController : ControllerBase
    {

        private readonly IRoleService _services;

        public RoleController(IRoleService services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _services.GetRolesAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<ApplicationRoleDTO>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Roles successfully."
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRoleDTO commend)
        {
            var result = await _services.CreateRoleAsync(commend.RoleName);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Message = "Create Roles successfully."
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ApplicationRoleDTO commend)
        {
            commend.Id = id;
            var result = await _services.UpdateRole(id, commend.RoleName);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result,
                Status = HttpStatusCode.Created,
                Message = "Update Roles successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _services.GetRoleByIdAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<ApplicationRoleDTO>
            {
                Success = true,
                Status = HttpStatusCode.Created,
                Data = result,
                Message = " Get Role successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _services.DeleteRoleAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Role Delete successfully."
            });
        }
    }
}
