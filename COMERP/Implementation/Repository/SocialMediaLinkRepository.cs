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
    public class SocialMediaLinkRepository : Repository<SocialMediaLink>, ISocialMediaLinkRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SocialMediaLinkRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddSocialMediaLinkSqlAsync(SocialMediaLinkDto model)
        {
            const string sql = @"
        INSERT INTO SocialMediaLinks 
        (Id, Platform, LinkUrl, IconUrl, DisplayOrder, IsVisible, CompanyId, CreationDate, CreatedBy)
        VALUES (@Id, @Platform, @LinkUrl, @IconUrl, @DisplayOrder, @IsVisible, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@Platform", model.Platform);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@IconUrl", model.IconUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Social media link added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add social media link.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateSocialMediaLinkSqlAsync(SocialMediaLinkDto model)
        {
            const string sql = @"
        UPDATE SocialMediaLinks
        SET Platform = @Platform,
            LinkUrl = @LinkUrl,
            IconUrl = @IconUrl,
            DisplayOrder = @DisplayOrder,
            IsVisible = @IsVisible,
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
                        parameters.Add("@Platform", model.Platform);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@IconUrl", model.IconUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Social media link not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Social media link updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update social media link.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<SocialMediaLink>> GetSocialMediaLinkSqlAsync()
        {
            const string sql = @"
        SELECT 
            sml.Id, sml.Platform, sml.LinkUrl, sml.IconUrl, sml.DisplayOrder, sml.IsVisible, sml.CompanyId,
            sml.CreationDate, sml.CreatedBy, sml.UpdateDate, sml.UpdatedBy,
            c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website,
            c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy
        FROM SocialMediaLinks sml
        INNER JOIN Companys c ON sml.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<SocialMediaLink, Company, SocialMediaLink>(
                        sql,
                        (sml, company) =>
                        {
                            sml.Company = company;
                            return sml;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve social media links.", ex);
                }
            }
        }

        public async Task<SocialMediaLink> GetSocialMediaLinkByIdSqlAsync(string id)
        {
            const string sql = @"
                SELECT 
                    sml.Id, sml.Platform, sml.LinkUrl, sml.IconUrl, sml.DisplayOrder, sml.IsVisible, sml.CompanyId,
                    sml.CreationDate, sml.CreatedBy, sml.UpdateDate, sml.UpdatedBy,
                    c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website,
                    c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy
                FROM SocialMediaLinks sml
                INNER JOIN Companys c ON sml.CompanyId = c.Id
                WHERE sml.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<SocialMediaLink, Company, SocialMediaLink>(
                        sql,
                        (sml, company) =>
                        {
                            sml.Company = company;
                            return sml;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve social media link.", ex);
                }
            }
        }

    }
}
