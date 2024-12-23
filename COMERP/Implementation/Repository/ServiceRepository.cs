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
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddServiceSqlAsync(ServiceDto model)
        {
            const string sql = @"
        INSERT INTO Services 
        (Id, Name, Description, Price, DurationHours, CompanyId, CreatedDate, CreatedBy)
        VALUES 
        (@Id, @Name, @Description, @Price, @DurationHours, @CompanyId, @CreatedDate, @CreatedBy);";

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
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@Price", model.Price);
                        parameters.Add("@DurationHours", model.DurationHours);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Service added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add service.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateServiceSqlAsync(ServiceDto model)
        {
            const string sql = @"
        UPDATE Services
        SET Name = @Name,
            Description = @Description,
            Price = @Price,
            DurationHours = @DurationHours,
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
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@Price", model.Price);
                        parameters.Add("@DurationHours", model.DurationHours);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Service not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Service updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update service.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<Service>> GetServiceSqlAsync()
        {
            const string sql = @"
        SELECT 
            s.Id, s.Name, s.Description, s.Price, s.DurationHours, s.CompanyId, s.CreatedDate,
            c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website
        FROM Services s
        INNER JOIN Companys c ON s.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Service, Company, Service>(
                        sql,
                        (service, company) =>
                        {
                            service.Company = company;
                            return service;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve services.", ex);
                }
            }
        }

        public async Task<Service> GetServiceByIdSqlAsync(string id)
        {
            const string sql = @"
        SELECT 
            s.Id, s.Name, s.Description, s.Price, s.DurationHours, s.CompanyId, s.CreatedDate,
            c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website
        FROM Services s
        INNER JOIN Companys c ON s.CompanyId = c.Id
        WHERE s.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Service, Company, Service>(
                        sql,
                        (service, company) =>
                        {
                            service.Company = company;
                            return service;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve the service.", ex);
                }
            }
        }

    }
}
