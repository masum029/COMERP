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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
         _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddCompanySqlAsync(CompanyDto model)
        {
            const string sql = @"
            INSERT INTO Companys (Id, Name, Description, EstablishedDate, ContactEmail, Phone, Address, Website,Logo,isActive, CreationDate, CreatedBy)
            VALUES (@Id, @Name, @Description, @EstablishedDate, @ContactEmail, @Phone, @Address, @Website,@Logo,@isActive, @CreationDate, @CreatedBy);
            SELECT CAST(SCOPE_IDENTITY() AS varchar);";



            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create a unique ID for the company
                        var companyId = Guid.NewGuid().ToString();

                        // Prepare parameters for the SQL query
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", companyId);
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@EstablishedDate", model.EstablishedDate);
                        parameters.Add("@ContactEmail", model.ContactEmail);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@Website", model.Website);
                        parameters.Add("@Logo", model.Logo);
                        parameters.Add("@isActive", model.isActive);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName()); // Assume this method fetches the username of the currently logged-in user.

                        // Execute the insert query
                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        // Commit the transaction if successful
                        transaction.Commit();

                        return (true, companyId, "Company added successfully.");
                    }
                    catch (Exception)
                    {
                        // Rollback transaction if there's an error
                        transaction.Rollback();
                        throw ;
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateCompanySqlAsync(CompanyDto model)
        {
            // SQL query to update the company details
            const string sql = @"
                            UPDATE Companys 
                            SET Name = @Name, 
                                Description = @Description, 
                                EstablishedDate = @EstablishedDate, 
                                ContactEmail = @ContactEmail, 
                                Phone = @Phone, 
                                Address = @Address, 
                                Website = @Website, 
                                Logo = @Logo, 
                                isActive = @isActive, 
                                UpdateDate = @UpdatedDate, 
                                UpdatedBy = @UpdatedBy 
                            WHERE Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open(); // Open the database connection

                using (var transaction = connection.BeginTransaction()) // Begin the transaction
                {
                    try
                    {
                        // Prepare parameters for the SQL query
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", model.Id);
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@Description", model.Description);
                        parameters.Add("@EstablishedDate", model.EstablishedDate);
                        parameters.Add("@ContactEmail", model.ContactEmail);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@Website", model.Website);
                        parameters.Add("@Logo", model.Logo);
                        parameters.Add("@isActive", model.isActive);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName()); // Retrieve the username of the logged-in user

                        // Execute the update query
                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        // Check if the company was updated
                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Company not found.");
                        }

                        // Commit the transaction if the update was successful
                        transaction.Commit();

                        return (true, model.Id, "Company updated successfully.");
                    }
                    catch (Exception )
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        throw ;
                    }
                }
            }
        }
    }
}
