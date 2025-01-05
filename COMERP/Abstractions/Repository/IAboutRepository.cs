using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IAboutRepository : IRepository<About>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddAboutSqlAsync(AboutDto model);
        Task<(bool Success, string id, string Message)> UpdateAboutSqlAsync(AboutDto model);
        Task<IEnumerable<About>> GetAboutSqlAsync();
        Task<About> GetAboutByIdSqlAsync(string id);
    }
}
