using COMERP.Abstractions.Repository;
using COMERP.DataContext;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Exceptions;
using COMERP.Implementation.Repository.Base;
using Dapper;
using System.Security.Claims;

namespace COMERP.Implementation.Repository
{
    public class SiteSettingsRepository : Repository<SiteSettings>, ISiteSettingsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SiteSettingsRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddSiteSettingsSqlAsync(SiteSettingsDto model)
        {
            const string sql = @"
        INSERT INTO SiteSettingss
        (Id, CompanyId, LogoUrl, FaviconUrl, ContactEmail, Phone, Address, CreatedDate, CreatedBy)
        VALUES (@Id, @CompanyId, @LogoUrl, @FaviconUrl, @ContactEmail, @Phone, @Address, @CreatedDate, @CreatedBy);";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var id = Guid.NewGuid().ToString();

                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", id);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@LogoUrl", model.LogoUrl);
                        parameters.Add("@FaviconUrl", model.FaviconUrl);
                        parameters.Add("@ContactEmail", model.ContactEmail);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Site settings added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add site settings.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateSiteSettingsSqlAsync(SiteSettingsDto model)
        {
            const string sql = @"
            UPDATE SiteSettingss
            SET LogoUrl = @LogoUrl,
                FaviconUrl = @FaviconUrl,
                ContactEmail = @ContactEmail,
                Phone = @Phone,
                Address = @Address,
                UpdateDate = @UpdateDate,
                UpdatedBy = @UpdatedBy
            WHERE Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", model.Id);
                        parameters.Add("@LogoUrl", model.LogoUrl);
                        parameters.Add("@FaviconUrl", model.FaviconUrl);
                        parameters.Add("@ContactEmail", model.ContactEmail);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Site settings not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Site settings updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update site settings.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<SiteSettings>> GetSiteSettingsSqlAsync()
        {
            const string sql = @"
        SELECT 
            s.Id, s.CompanyId, s.LogoUrl, s.FaviconUrl, s.ContactEmail, s.Phone, s.Address, s.CreatedDate,
            c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website
        FROM SiteSettingss s
        INNER JOIN Companys c ON s.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<SiteSettings, Company, SiteSettings>(
                        sql,
                        (siteSettings, company) =>
                        {
                            siteSettings.Company = company;
                            return siteSettings;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve site settings.", ex);
                }
            }
        }

        public async Task<SiteSettings> GetSiteSettingsByIdSqlAsync(string id)
        {
            const string sql = @"
            SELECT 
                s.Id, s.CompanyId, s.LogoUrl, s.FaviconUrl, s.ContactEmail, s.Phone, s.Address, s.CreatedDate,
                c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website
            FROM SiteSettingss s
            INNER JOIN Companys c ON s.CompanyId = c.Id
            WHERE s.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<SiteSettings, Company, SiteSettings>(
                        sql,
                        (siteSettings, company) =>
                        {
                            siteSettings.Company = company;
                            return siteSettings;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve site settings.", ex);
                }
            }
        }

    }
}
