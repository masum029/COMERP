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
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";

        public async Task<(bool Success, string id, string Message)> AddEventSqlAsync(EventDto model)
        {
            const string sql = @"
            INSERT INTO Events
            (Id, Title, Description, EventDate, Location, CompanyId, CreationDate, CreatedBy)
            VALUES
            (@Id, @Title, @Description, @EventDate, @Location, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@EventDate", model.EventDate);
                        parameters.Add("@Location", model.Location);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();
                        return (true, id, "Event added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add event.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateEventSqlAsync(EventDto model)
        {
            const string sql = @"
            UPDATE Events
            SET Title = @Title,
                Description = @Description,
                EventDate = @EventDate,
                Location = @Location,
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
                        parameters.Add("@Title", model.Title);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@EventDate", model.EventDate);
                        parameters.Add("@Location", model.Location);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Event not found.");
                        }

                        transaction.Commit();
                        return (true, model.Id, "Event updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update event.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<Event>> GetEventSqlAsync()
        {
            const string sql = @"
            SELECT
                e.Id, e.Title, e.Description, e.EventDate, e.Location, e.CompanyId,
                c.Id, c.Name
            FROM Events e
            INNER JOIN Companys c ON e.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<Event, Company, Event>(
                        sql,
                        (eventEntity, company) =>
                        {
                            eventEntity.Company = company;
                            return eventEntity;
                        },
                        splitOn: "CompanyId"
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve events.", ex);
                }
            }
        }

        public async Task<Event> GetEventByIdSqlAsync(string id)
        {
            const string sql = @"
            SELECT
                e.Id, e.Title, e.Description, e.EventDate, e.Location, e.CompanyId,
                c.Id, c.Name
            FROM Events e
            INNER JOIN Companys c ON e.CompanyId = c.Id
            WHERE e.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<Event, Company, Event>(
                        sql,
                        (eventEntity, company) =>
                        {
                            eventEntity.Company = company;
                            return eventEntity;
                        },
                        param: new { Id = id },
                        splitOn: "CompanyId"
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve event by ID.", ex);
                }
            }
        }

    }
}
