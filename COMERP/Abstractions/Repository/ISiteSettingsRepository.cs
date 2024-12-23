using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface ISiteSettingsRepository : IRepository<SiteSettings>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddSiteSettingsSqlAsync(SiteSettingsDto model);
        Task<(bool Success, string id, string Message)> UpdateSiteSettingsSqlAsync(SiteSettingsDto model);
        Task<IEnumerable<SiteSettings>> GetSiteSettingsSqlAsync();
        Task<SiteSettings> GetSiteSettingsByIdSqlAsync(string id);
    }
}
