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
    public class ContactFormSubmissionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactFormSubmissionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ContactFormSubmissionDto model)
        {
            var result = await _unitOfWork.contactFormSubmissionRepository.AddContactFormSubmissionSqlAsync(model);
            return StatusCode((int)HttpStatusCode.Created, new Response<string>
            {
                Success = result.Success,
                Status = HttpStatusCode.Created,
                id = result.id,
                Message = result.Message,
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ContactFormSubmissionDto model)
        {
            model.Id = id;
            var result = await _unitOfWork.contactFormSubmissionRepository.UpdateContactFormSubmissionSqlAsync(model);
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
            var result = await _unitOfWork.contactFormSubmissionRepository.GetContactFormSubmissionSqlAsync();
            return StatusCode((int)HttpStatusCode.OK, new Response<IEnumerable<ContactFormSubmission>>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = "Get All Contact Form Submission successfully."
            });
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id)
        {
            var result = await _unitOfWork.contactFormSubmissionRepository.GetContactFormSubmissionByIdSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<ContactFormSubmission>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = result,
                Message = " Get Contact Form Submission successfully."
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _unitOfWork.contactFormSubmissionRepository.DeleteSqlAsync(id);
            return StatusCode((int)HttpStatusCode.OK, new Response<string>
            {
                Success = true,
                Status = HttpStatusCode.OK,
                Data = "Success",
                Message = "Contact Form Submission Delete successfully."
            });
        }
    }
}
