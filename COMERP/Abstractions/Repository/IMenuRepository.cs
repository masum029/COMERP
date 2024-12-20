using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IMenuRepository : IRepository<Menu>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddMenuSqlAsync(MenuDto model);
        Task<(bool Success, string id, string Message)> UpdateMenuSqlAsync(MenuDto model);
    }
}
