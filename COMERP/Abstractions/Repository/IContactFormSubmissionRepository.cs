using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IContactFormSubmissionRepository : IRepository<ContactFormSubmission>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddContactFormSubmissionSqlAsync(ContactFormSubmissionDto model);
        Task<(bool Success, string id, string Message)> UpdateContactFormSubmissionSqlAsync(ContactFormSubmissionDto model);
    }
}
