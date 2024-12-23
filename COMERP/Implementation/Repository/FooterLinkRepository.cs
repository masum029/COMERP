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
    public class FooterLinkRepository : Repository<FooterLink>, IFooterLinkRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FooterLinkRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddFooterLinkSqlAsync(FooterLinkDto model)
        {
            const string sql = @"
        INSERT INTO FooterLinks
        (Id, Title, LinkUrl, DisplayOrder, IsVisible, CompanyId, CreatedDate, CreatedBy)
        VALUES
        (@Id, @Title, @LinkUrl, @DisplayOrder, @IsVisible, @CompanyId, @CreatedDate, @CreatedBy);";

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
                        parameters.Add("@Title", model.Title);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();
                        return (true, id, "Footer link added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add footer link.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateFooterLinkSqlAsync(FooterLinkDto model)
        {
            const string sql = @"
        UPDATE FooterLinks
        SET Title = @Title,
            LinkUrl = @LinkUrl,
            DisplayOrder = @DisplayOrder,
            IsVisible = @IsVisible,
            CompanyId = @CompanyId,
            UpdatedDate = @UpdatedDate,
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
                        parameters.Add("@Title", model.Title);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Footer link not found.");
                        }

                        transaction.Commit();
                        return (true, model.Id, "Footer link updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update footer link.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<FooterLink>> GetFooterLinkSqlAsync()
        {
            const string sql = @"
                SELECT
                    f.Id, f.Title, f.LinkUrl, f.DisplayOrder, f.IsVisible, f.CompanyId,
                    c.Id, c.Name
                FROM FooterLinks f
                INNER JOIN Companys c ON f.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<FooterLink, Company, FooterLink>(
                        sql,
                        (footerLink, company) =>
                        {
                            footerLink.Company = company;
                            return footerLink;
                        },
                        splitOn: "CompanyId"
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve footer links.", ex);
                }
            }
        }

        public async Task<FooterLink> GetFooterLinkByIdSqlAsync(string id)
        {
            const string sql = @"
                SELECT
                    f.Id, f.Title, f.LinkUrl, f.DisplayOrder, f.IsVisible, f.CompanyId,
                    c.Id, c.Name
                FROM FooterLinks f
                INNER JOIN Companys c ON f.CompanyId = c.Id
                WHERE f.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<FooterLink, Company, FooterLink>(
                        sql,
                        (footerLink, company) =>
                        {
                            footerLink.Company = company;
                            return footerLink;
                        },
                        param: new { Id = id },
                        splitOn: "CompanyId"
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve footer link by ID.", ex);
                }
            }
        }

    }
}
