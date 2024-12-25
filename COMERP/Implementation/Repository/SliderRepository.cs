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
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SliderRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddSliderSqlAsync(SliderDto model)
        {
            const string sql = @"
        INSERT INTO Sliders
        (Id, Title, Subtitle, ImageUrl, LinkUrl, DisplayOrder, IsActive, CreatedDate, CompanyId, CreatedBy)
        VALUES (@Id, @Title, @Subtitle, @ImageUrl, @LinkUrl, @DisplayOrder, @IsActive, @CreatedDate, @CompanyId, @CreatedBy);";

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
                        parameters.Add("@Subtitle", model.Subtitle);
                        parameters.Add("@ImageUrl", model.ImageUrl);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsActive", model.IsActive);
                        parameters.Add("@CreatedDate", DateTime.UtcNow);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Slider added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add slider.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateSliderSqlAsync(SliderDto model)
        {
            const string sql = @"
                UPDATE Sliders
                SET Title = @Title,
                    Subtitle = @Subtitle,
                    ImageUrl = @ImageUrl,
                    LinkUrl = @LinkUrl,
                    DisplayOrder = @DisplayOrder,
                    IsActive = @IsActive,
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
                        parameters.Add("@Subtitle", model.Subtitle);
                        parameters.Add("@ImageUrl", model.ImageUrl);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsActive", model.IsActive);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Slider not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Slider updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update slider.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<Slider>> GetSlidersSqlAsync()
        {
            const string sql = @"
        SELECT 
            s.Id, s.Title, s.Subtitle, s.ImageUrl, s.LinkUrl, s.DisplayOrder, s.IsActive, s.CreatedDate,
            s.CompanyId, c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website
            ,c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy
        FROM Sliders s
        INNER JOIN Companys c ON s.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Slider, Company, Slider>(
                        sql,
                        (slider, company) =>
                        {
                            slider.Company = company;
                            return slider;
                        }
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve sliders.", ex);
                }
            }
        }

        public async Task<Slider> GetSliderByIdSqlAsync(string id)
        {
            const string sql = @"
                SELECT 
                    s.Id, s.Title, s.Subtitle, s.ImageUrl, s.LinkUrl, s.DisplayOrder, s.IsActive, s.CreatedDate,
                    s.CompanyId, c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website 
                    , c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy
                FROM Sliders s
                INNER JOIN Companys c ON s.CompanyId = c.Id
                WHERE s.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Slider, Company, Slider>(
                        sql,
                        (slider, company) =>
                        {
                            slider.Company = company;
                            return slider;
                        },
                        param: new { Id = id }
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve slider.", ex);
                }
            }
        }

    }
}
