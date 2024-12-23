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
    public class SliderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SliderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderDto model)
        {
            var result = await _unitOfWork.sliderRepository.AddSliderSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, SliderDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.sliderRepository.UpdateSliderSqlAsync(model);
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
            var result = await _unitOfWork.sliderRepository.GetSlidersSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<Slider>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Slider successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.sliderRepository.GetSliderByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<Slider>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Slider successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.sliderRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Slider Delete successfully."
            });
        }
    }
}
