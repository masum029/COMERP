using COMERP.Abstractions.Interfaces;
using COMERP.Common;
using COMERP.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace COMERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUsersAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<UserDTO>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Users retrieved successfully."
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDetails(string Id)
        {
            var user = await _userService.GetUserDetailsAsync(Id);
            if (user.Id == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, new Response<UserDTO>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Message = "User not found."
                });
            }
            var response = new Response<UserDTO>
            {
                Success = true,
                Data = user,
                Status = HttpStatusCode.OK,
                Message = "User details retrieved successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _userService.DeleteUserAsync(Id);
            if (!result)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response<string>
                {
                    Success = false,
                    Status = HttpStatusCode.InternalServerError,
                    Message = "An error occurred while deleting the user."
                });
            }

            var response = new Response<string>
            {
                Success = true,
                Data = Id,
                Status = HttpStatusCode.OK,
                Message = "User deleted successfully."
            };

            return StatusCode((int)HttpStatusCode.OK, response);
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> Edit(string Id, UserDTO model)
        {
            // Attempt to update the user profile
            var updateUser = await _userService.UpdateUserProfile(model);
            if (updateUser)
            {
                return StatusCode((int)HttpStatusCode.Created, new Response<string>
                {
                    Success = true,
                    Status = HttpStatusCode.Created,
                    Message = "User profile updated successfully."
                });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, new Response<string>
                {
                    Success = false,
                    Status = HttpStatusCode.NotFound,
                    Message = "User not found or update failed."
                });
            }
        }
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var (success, errorMessage) = await _userService.ChangePassword(model.OldPassword, model.NewPassword, model.UserId);

            if (success)
            {
                return Ok(new Response<string>
                {
                    Success = true,
                    Status = HttpStatusCode.OK,
                    Message = "Password changed successfully."
                });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new Response<string>
                {
                    Success = false,
                    Status = HttpStatusCode.BadRequest,
                    Message = errorMessage
                });
            }
        }
    }
}
