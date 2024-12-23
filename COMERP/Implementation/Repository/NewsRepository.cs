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
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddNewsSqlAsync(NewsDto model)
        {
            const string sql = @"
        INSERT INTO Newss
        (Id, Title, Content, PublishedDate, CompanyId, CreatedDate, CreatedBy)
        VALUES 
        (@Id, @Title, @Content, @PublishedDate, @CompanyId, @CreatedDate, @CreatedBy);";

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
                        parameters.Add("@Content", model.Content);
                        parameters.Add("@PublishedDate", model.PublishedDate);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "News added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add news.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateNewsSqlAsync(NewsDto model)
        {
            const string sql = @"
        UPDATE Newss
        SET Title = @Title,
            Content = @Content,
            PublishedDate = @PublishedDate,
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
                        parameters.Add("@Content", model.Content);
                        parameters.Add("@PublishedDate", model.PublishedDate);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("News not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "News updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update news.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<News>> GetNewsSqlAsync()
        {
            const string sql = @"
        SELECT 
            n.Id, n.Title, n.Content, n.PublishedDate, n.CompanyId, n.CreatedDate,
            c.Id, c.Name, c.ContactEmail, c.Phone
        FROM Newss n
        INNER JOIN Companys c ON n.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<News, Company, News>(
                        sql,
                        (news, company) =>
                        {
                            news.Company = company;
                            return news;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve news.", ex);
                }
            }
        }

        public async Task<News> GetNewsByIdSqlAsync(string id)
        {
            const string sql = @"
        SELECT 
            n.Id, n.Title, n.Content, n.PublishedDate, n.CompanyId, n.CreatedDate,
            c.Id, c.Name, c.ContactEmail, c.Phone
        FROM Newss n
        INNER JOIN Companys c ON n.CompanyId = c.Id
        WHERE n.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<News, Company, News>(
                        sql,
                        (news, company) =>
                        {
                            news.Company = company;
                            return news;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve news.", ex);
                }
            }
        }

    }
}
