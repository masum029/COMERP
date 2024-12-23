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
    public class MenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuDto model)
        {
            var result = await _unitOfWork.menuRepository.AddMenuSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, MenuDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.menuRepository.UpdateMenuSqlAsync(model);
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
            var result = await _unitOfWork.menuRepository.GetMenuSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<Menu>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Menu successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.menuRepository.GetMenuByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<Menu>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Menu successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.menuRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Menu Delete successfully."
            });
        }
    }
}
