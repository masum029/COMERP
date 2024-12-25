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
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddProjectSqlAsync(ProjectDto model)
        {
            const string sql = @"
        INSERT INTO Projects 
        (Id, Name, Description, StartDate, EndDate, Status, ClientId, CompanyId, CreationDate, CreatedBy)
        VALUES 
        (@Id, @Name, @Description, @StartDate, @EndDate, @Status, @ClientId, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@StartDate", model.StartDate);
                        parameters.Add("@EndDate", model.EndDate);
                        parameters.Add("@Status", model.Status);
                        parameters.Add("@ClientId", model.ClientId);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Project added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add project.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateProjectSqlAsync(ProjectDto model)
        {
            const string sql = @"
                UPDATE Projects
                SET Name = @Name,
                    Description = @Description,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Status = @Status,
                    ClientId = @ClientId,
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
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@StartDate", model.StartDate);
                        parameters.Add("@EndDate", model.EndDate);
                        parameters.Add("@Status", model.Status);
                        parameters.Add("@ClientId", model.ClientId);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Project not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Project updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ;
                    }
                }
            }
        }

        public async Task<IEnumerable<Project>> GetProjectSqlAsync()
        {
            const string sql = @"
        SELECT 
            p.Id, p.Name, p.Description, p.StartDate, p.EndDate, p.Status, p.ClientId, p.CompanyId,
            c.Id, c.Name, c.Address, c.Phone, c.Email, c.CompanyId,
            cmp.Id, cmp.Name, cmp.ContactEmail, cmp.Phone
        FROM Projects p
        INNER JOIN Companys cmp ON p.CompanyId = cmp.Id
        LEFT JOIN Clients c ON p.ClientId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Project, Client, Company, Project>(
                        sql,
                        (project, client, company) =>
                        {
                            project.Client = client;
                            project.Company = company;
                            return project;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw ;
                }
            }
        }

        public async Task<Project> GetProjectByIdSqlAsync(string id)
        {
            const string sql = @"
            SELECT 
                p.Id, p.Name, p.Description, p.StartDate, p.EndDate, p.Status, p.ClientId, p.CompanyId,
                c.Id, c.Name, c.Address, c.Phone, c.Email, c.CompanyId,
                cmp.Id, cmp.Name, cmp.ContactEmail, cmp.Phone
            FROM Projects p
            INNER JOIN Companys cmp ON p.CompanyId = cmp.Id
            LEFT JOIN Clients c ON p.ClientId = c.Id
            WHERE p.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Project, Client, Company, Project>(
                        sql,
                        (project, client, company) =>
                        {
                            project.Client = client;
                            project.Company = company;
                            return project;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ;
                }
            }
        }

    }
}
