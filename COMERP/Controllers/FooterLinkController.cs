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
    public class FooterLinkController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FooterLinkController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(FooterLinkDto model)
        {
            var result = await _unitOfWork.footerLinkRepository.AddFooterLinkSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, FooterLinkDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.footerLinkRepository.UpdateFooterLinkSqlAsync(model);
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
            var result = await _unitOfWork.footerLinkRepository.GetFooterLinkSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<FooterLink>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Footer Link successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.footerLinkRepository.GetFooterLinkByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<FooterLink>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Footer Link successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.footerLinkRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Footer Link Delete successfully."
            });
        }
    }
}
