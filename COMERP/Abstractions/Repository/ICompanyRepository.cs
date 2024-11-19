using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddCompanySqlAsync(CompanyDto model);
        Task<(bool Success, string id, string Message)> UpdateCompanySqlAsync(CompanyDto model);
    }
}
