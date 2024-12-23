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
    public class EventController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(EventDto model)
        {
            var result = await _unitOfWork.eventRepository.AddEventSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, EventDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.eventRepository.UpdateEventSqlAsync(model);
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
            var result = await _unitOfWork.eventRepository.GetEventSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<Event>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Event successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.eventRepository.GetEventByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<Event>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Event successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.eventRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Event Delete successfully."
            });
        }
    }
}
