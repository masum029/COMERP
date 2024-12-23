using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface INewsRepository : IRepository<News>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddNewsSqlAsync(NewsDto model);
        Task<(bool Success, string id, string Message)> UpdateNewsSqlAsync(NewsDto model);
        Task<IEnumerable<News>> GetNewsSqlAsync();
        Task<News> GetNewsByIdSqlAsync(string id);
    }
}
