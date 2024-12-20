using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface ICompanyDetailsRepository : IRepository<CompanyDetails>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddCompanyDetailsSqlAsync(CompanyDetailsDto model);
        Task<(bool Success, string id, string Message)> UpdateCompanyDetailsSqlAsync(CompanyDetailsDto model);
    }
}
