using COMERP.Abstractions.Interfaces;
using COMERP.Common;
using COMERP.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace COMERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _services;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IUserService services, ITokenGenerator tokenGenerator)
        {
            _services = services;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Create(RegisterUserDTO commend)
        {
            var result = await _services.CreateUserAsync(commend);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.Created,
                id = result.userId,
                Message = "Succeed: User registered successfully!"
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO commend)
        {
            var result = await _services.SigninUserAsync(commend.UserName, commend.Password);
            if (!result)
            {
                throw new Exception("Invalid username or password");
            }
            var user = await _services.GetUserDetailsAsync(await _services.GetUserIdAsync(commend.UserName));

            string token = _tokenGenerator.GenerateJWTToken((user.Id, user.UserName, user.FirstName, user.LastName, user.Email, user.Roles));
            var auth = new AuthDTO()
            {
                UserId = user.Id,
                Name = user.UserName,
                Token = token,
            };
            // Return the response with user data
            return StatusCode((int)HttpStatusCode.Created, new Response<AuthDTO>
            {
                Success = true,
                Data = auth,
                Status = HttpStatusCode.OK,
                Message = "User Lgoin successfully."
            });
        }
    }
}
