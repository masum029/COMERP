using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IPageContentRepository : IRepository<PageContent>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddPageContentSqlAsync(PageContentDto model);
        Task<(bool Success, string id, string Message)> UpdatePageContentSqlAsync(PageContentDto model);
    }
}
