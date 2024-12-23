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
    public class CompanyDetailsRepository : Repository<CompanyDetails>, ICompanyDetailsRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyDetailsRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddCompanyDetailsSqlAsync(CompanyDetailsDto model)
        {
            const string sql = @"
        INSERT INTO CompanyDetailss (Id, CompanyId, Mission, Vision, CoreValues, CreationDate, CreatedBy)
        VALUES (@Id, @CompanyId, @Mission, @Vision, @CoreValues, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@Mission", model.Mission);
                        parameters.Add("@Vision", model.Vision);
                        parameters.Add("@CoreValues", model.CoreValues);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Company details added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add company details.", ex);
                    }
                }
            }
        }

        public async Task<CompanyDetails> GetCompanyDetailsByIdSqlAsync(string id)
        {
            const string sql = @"
                SELECT 
                    cd.Id, cd.CompanyId, cd.Mission, cd.Vision, cd.CoreValues, cd.CreationDate, cd.CreatedBy, cd.UpdateDate, cd.UpdatedBy,
                    c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website, 
                    c.CreationDate , c.CreatedBy , c.UpdateDate , c.UpdatedBy 
                FROM CompanyDetailss cd
                INNER JOIN Companys c ON cd.CompanyId = c.Id
                WHERE cd.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<CompanyDetails, Company, CompanyDetails>(
                        sql,
                        (details, company) =>
                        {
                            details.Company = company;
                            return details;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve company details.", ex);
                }
            }
        }

        public async Task<IEnumerable<CompanyDetails>> GetCompanyDetailsSqlAsync()
        {
            const string sql = @"
                SELECT 
                    cd.Id, cd.CompanyId, cd.Mission, cd.Vision, cd.CoreValues, cd.CreationDate, cd.CreatedBy, cd.UpdateDate, cd.UpdatedBy,
                    c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website, 
                    c.CreationDate , c.CreatedBy , c.UpdateDate , c.UpdatedBy 
                FROM CompanyDetailss cd
                INNER JOIN Companys c ON cd.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<CompanyDetails, Company, CompanyDetails>(
                        sql,
                        (details, company) =>
                        {
                            details.Company = company;
                            return details;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve company details.", ex);
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateCompanyDetailsSqlAsync(CompanyDetailsDto model)
        {
            const string sql = @"
                UPDATE CompanyDetailss
                SET Mission = @Mission,
                    Vision = @Vision,
                    CoreValues = @CoreValues,
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
                        parameters.Add("@Mission", model.Mission);
                        parameters.Add("@Vision", model.Vision);
                        parameters.Add("@CoreValues", model.CoreValues);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Company details not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Company details updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update company details.", ex);
                    }
                }
            }
        }

    }
}
