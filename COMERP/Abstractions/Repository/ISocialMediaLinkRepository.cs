using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface ISocialMediaLinkRepository : IRepository<SocialMediaLink>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddSocialMediaLinkSqlAsync(SocialMediaLinkDto model);
        Task<(bool Success, string id, string Message)> UpdateSocialMediaLinkSqlAsync(SocialMediaLinkDto model);
    }
}
