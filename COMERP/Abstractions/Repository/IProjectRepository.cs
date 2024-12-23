using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IProjectRepository : IRepository<Project>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddProjectSqlAsync(ProjectDto model);
        Task<(bool Success, string id, string Message)> UpdateProjectSqlAsync(ProjectDto model);
        Task<IEnumerable<Project>> GetProjectSqlAsync();
        Task<Project> GetProjectByIdSqlAsync(string id);
    }
}
