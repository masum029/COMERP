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
    public class PageContentRepository : Repository<PageContent>, IPageContentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageContentRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddPageContentSqlAsync(PageContentDto model)
        {
            const string sql = @"
        INSERT INTO PageContents 
        (Id, PageName, SectionTitle, Content, ImageUrl, DisplayOrder, IsVisible, CompanyId, CreationDate, CreatedBy)
        VALUES 
        (@Id, @PageName, @SectionTitle, @Content, @ImageUrl, @DisplayOrder, @IsVisible, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@PageName", model.PageName);
                        parameters.Add("@SectionTitle", model.SectionTitle);
                        parameters.Add("@Content", model.Content);
                        parameters.Add("@ImageUrl", model.ImageUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Page content added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add page content.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdatePageContentSqlAsync(PageContentDto model)
        {
            const string sql = @"
            UPDATE PageContents
            SET PageName = @PageName,
                SectionTitle = @SectionTitle,
                Content = @Content,
                ImageUrl = @ImageUrl,
                DisplayOrder = @DisplayOrder,
                IsVisible = @IsVisible,
                CompanyId = @CompanyId,
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
                        parameters.Add("@PageName", model.PageName);
                        parameters.Add("@SectionTitle", model.SectionTitle);
                        parameters.Add("@Content", model.Content);
                        parameters.Add("@ImageUrl", model.ImageUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Page content not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Page content updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update page content.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<PageContent>> GetPageContentSqlAsync()
        {
            const string sql = @"
        SELECT 
            p.Id, p.PageName, p.SectionTitle, p.Content, p.ImageUrl, p.DisplayOrder, p.IsVisible, p.CompanyId,
            c.Id, c.Name, c.ContactEmail, c.Phone
        FROM PageContents p
        INNER JOIN Companys c ON p.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<PageContent, Company, PageContent>(
                        sql,
                        (pageContent, company) =>
                        {
                            pageContent.Company = company;
                            return pageContent;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve page contents.", ex);
                }
            }
        }

        public async Task<PageContent> GetPageContentByIdSqlAsync(string id)
        {
            const string sql = @"
            SELECT 
                p.Id, p.PageName, p.SectionTitle, p.Content, p.ImageUrl, p.DisplayOrder, p.IsVisible, p.CompanyId,
                c.Id, c.Name, c.ContactEmail, c.Phone
            FROM PageContents p
            INNER JOIN Companys c ON p.CompanyId = c.Id
            WHERE p.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<PageContent, Company, PageContent>(
                        sql,
                        (pageContent, company) =>
                        {
                            pageContent.Company = company;
                            return pageContent;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve page content.", ex);
                }
            }
        }

    }
}
