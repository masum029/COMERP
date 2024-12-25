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
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MenuRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddMenuSqlAsync(MenuDto model)
        {
            const string sql = @"
        INSERT INTO Menus
        (Id, ParentMenuId, Title, LinkUrl, DisplayOrder, IsVisible, CompanyId, CreationDate, CreatedBy)
        VALUES 
        (@Id, @ParentMenuId, @Title, @LinkUrl, @DisplayOrder, @IsVisible, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@ParentMenuId", model.ParentMenuId);
                        parameters.Add("@Title", model.Title);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();

                        return (true, id, "Menu added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add menu.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateMenuSqlAsync(MenuDto model)
        {
            const string sql = @"
        UPDATE Menus
        SET ParentMenuId = @ParentMenuId,
            Title = @Title,
            LinkUrl = @LinkUrl,
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
                        parameters.Add("@ParentMenuId", model.ParentMenuId);
                        parameters.Add("@Title", model.Title);
                        parameters.Add("@LinkUrl", model.LinkUrl);
                        parameters.Add("@DisplayOrder", model.DisplayOrder);
                        parameters.Add("@IsVisible", model.IsVisible);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Menu not found.");
                        }

                        transaction.Commit();

                        return (true, model.Id, "Menu updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update menu.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<Menu>> GetMenuSqlAsync()
        {
            const string sql = @"
        SELECT 
            m.Id, m.ParentMenuId, m.Title, m.LinkUrl, m.DisplayOrder, m.IsVisible, m.CompanyId,
            p.Id, p.Title ,
            c.Id, c.Name
        FROM Menus m
        LEFT JOIN Menus p ON m.ParentMenuId = p.Id
        INNER JOIN Companys c ON m.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Menu, Menu, Company, Menu>(
                        sql,
                        (menu, parentMenu, company) =>
                        {
                            menu.ParentMenu = parentMenu;
                            menu.Company = company;
                            return menu;
                        }
                        
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve menus.", ex);
                }
            }
        }

        public async Task<Menu> GetMenuByIdSqlAsync(string id)
        {
            const string sql = @"
        SELECT 
            m.Id, m.ParentMenuId, m.Title, m.LinkUrl, m.DisplayOrder, m.IsVisible, m.CompanyId,
            p.Id, p.Title ,
            c.Id, c.Name
        FROM Menus m
        LEFT JOIN Menus p ON m.ParentMenuId = p.Id
        INNER JOIN Companys c ON m.CompanyId = c.Id
        WHERE m.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                try
                {
                    var result = await connection.QueryAsync<Menu, Menu, Company, Menu>(
                        sql,
                        (menu, parentMenu, company) =>
                        {
                            menu.ParentMenu = parentMenu;
                            menu.Company = company;
                            return menu;
                        },
                        param: new { Id = id }
                        
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve menu.", ex);
                }
            }
        }

    }
}
