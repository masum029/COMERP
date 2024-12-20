using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IFooterLinkRepository : IRepository<FooterLink>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddFooterLinkSqlAsync(FooterLinkDto model);
        Task<(bool Success, string id, string Message)> UpdateFooterLinkSqlAsync(FooterLinkDto model);
    }
}
