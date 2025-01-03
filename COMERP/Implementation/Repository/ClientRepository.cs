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
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        public async Task<(bool Success, string id, string Message)> AddClientSqlAsync(ClientDto model)
        {
            const string sql = @"
                    INSERT INTO Clients (Id, Name, ContactPerson, Email, Phone,Icon,isActive, Address, CompanyId, CreationDate, CreatedBy)
                    VALUES (@Id, @Name, @ContactPerson, @Email, @Phone,@Icon,@isActive, @Address, @CompanyId, @CreationDate, @CreatedBy);
                    SELECT CAST(SCOPE_IDENTITY() AS varchar);";



            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Create a unique ID for the client
                        var clientId = Guid.NewGuid().ToString();

                        // Prepare parameters for the SQL query
                        var parameters = new DynamicParameters();
                        parameters.Add("@Id", clientId);
                        parameters.Add("@Name", model.Name);
                        parameters.Add("@ContactPerson", model.ContactPerson);
                        parameters.Add("@Email", model.Email);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@Icon", model.Icon);
                        parameters.Add("@isActive", model.isActive);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        // Execute the insert query
                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        // Commit the transaction if successful
                        transaction.Commit();

                        return (true, clientId, "Client added successfully.");
                    }
                    catch (Exception)
                    {
                        // Rollback transaction if there's an error
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<(bool Success, string id, string Message)> UpdateClientSqlAsync(ClientDto model)
        {
            // SQL query to update the client details
            const string sql = @"
                    UPDATE Clients
                    SET Name = @Name,
                        ContactPerson = @ContactPerson,
                        Email = @Email,
                        Phone = @Phone,
                        Icon = @Icon,
                        isActive = @isActive,
                        Address = @Address,
                        CompanyId = @CompanyId,
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
                        parameters.Add("@ContactPerson", model.ContactPerson);
                        parameters.Add("@Email", model.Email);
                        parameters.Add("@Phone", model.Phone);
                        parameters.Add("@Address", model.Address);
                        parameters.Add("@Icon", model.Icon);
                        parameters.Add("@isActive", model.isActive);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdatedDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName()); // Retrieve the username of the logged-in user

                        // Execute the update query
                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        // Check if the client was updated
                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Client not found.");
                        }

                        // Commit the transaction if the update was successful
                        transaction.Commit();

                        return (true, model.Id, "Client updated successfully.");
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<Client>> GetClientSqlAsync()
        {
            const string sql = @"
                SELECT 
                    c.Id, c.Name, c.ContactPerson, c.Email,c.Icon,c.isActive, c.Phone, c.Address, c.CompanyId, 
                    c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy,
                    co.Id, co.Name, co.Description, co.EstablishedDate, co.ContactEmail, co.Phone ,
                    co.Address, co.Website, co.CreationDate ,co.CreationDate, co.CreatedBy, co.UpdateDate, co.UpdatedBy
                FROM Clients c
                INNER JOIN Companys co ON c.CompanyId = co.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {

                connection.Open(); // Open the database connection
                try
                {
                    // Multi-mapping with Dapper to include the Company object
                    var clients = await connection.QueryAsync<Client, Company, Client>(
                        sql,
                        (client, company) =>
                        {
                            client.Company = company; // Map the related Company object
                            return client;
                        }
                    );

                    return clients;
                }
                catch (Exception)
                {
                    throw ;
                }
            }
        }

        public async Task<Client> GetClientByIdSqlAsync(string id)
        {
            const string sql = @"
                    SELECT 
                        c.Id, c.Name, c.ContactPerson, c.Email, c.Phone, c.Address,c.Icon,c.isActive,c.CompanyId, 
                        c.CreationDate, c.CreatedBy, c.UpdateDate, c.UpdatedBy,
                        co.Id, co.Name, co.Description, co.EstablishedDate, co.ContactEmail, co.Phone ,
                        co.Address , co.Website, co.CreationDate ,
                        co.CreatedBy , co.UpdateDate , co.UpdatedBy 
                    FROM Clients c
                    INNER JOIN Companys co ON c.CompanyId = co.Id
                    WHERE c.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open(); // Open the database connection
                try
                {
                    // Multi-mapping with Dapper to include the Company object
                    var client = await connection.QueryAsync<Client, Company, Client>(
                        sql,
                        (client, company) =>
                        {
                            client.Company = company; // Map the related Company object
                            return client;
                        },
                        param: new { Id = id } // Pass the ID parameter
                    );

                    // Return the first record or null if not found
                    return client.FirstOrDefault();
                }
                catch (Exception )
                {
                    throw ;
                }
            }
        }

    }
}
