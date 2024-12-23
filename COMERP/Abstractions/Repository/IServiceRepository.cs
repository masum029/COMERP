using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IServiceRepository : IRepository<Service>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddServiceSqlAsync(ServiceDto model);
        Task<(bool Success, string id, string Message)> UpdateServiceSqlAsync(ServiceDto model);
        Task<IEnumerable<Service>> GetServiceSqlAsync();
        Task<Service> GetServiceByIdSqlAsync(string id);
    }
}
