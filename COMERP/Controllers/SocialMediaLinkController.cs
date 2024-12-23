using AutoMapper;
using COMERP.Abstractions.Repository;
using COMERP.Common;
using COMERP.DTOs;
using COMERP.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace COMERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaLinkController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SocialMediaLinkController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(SocialMediaLinkDto model)
        {
            var result = await _unitOfWork.socialMediaLinkRepository.AddSocialMediaLinkSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, SocialMediaLinkDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.socialMediaLinkRepository.UpdateSocialMediaLinkSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _unitOfWork.socialMediaLinkRepository.GetSocialMediaLinkSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<SocialMediaLink>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Social Media Link successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.socialMediaLinkRepository.GetSocialMediaLinkByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<SocialMediaLink>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Social Media Link successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.socialMediaLinkRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Social Media Link Delete successfully."
            });
        }
    }
}
