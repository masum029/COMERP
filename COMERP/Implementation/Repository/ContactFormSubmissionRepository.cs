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
    public class ContactFormSubmissionRepository : Repository<ContactFormSubmission>, IContactFormSubmissionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly DapperDbContext _dapperDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactFormSubmissionRepository(ApplicationDbContext applicationDbContext, DapperDbContext dapperDbContext, IHttpContextAccessor httpContextAccessor)
            : base(applicationDbContext, dapperDbContext, httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _dapperDbContext = dapperDbContext;
            _httpContextAccessor = httpContextAccessor;

        }
        private string GetUserName() =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";

        public async Task<(bool Success, string id, string Message)> AddContactFormSubmissionSqlAsync(ContactFormSubmissionDto model)
        {
            const string sql = @"
        INSERT INTO ContactFormSubmissions
        (Id, Name, Email, Message, SubmissionDate, CompanyId, CreationDate, CreatedBy)
        VALUES
        (@Id, @Name, @Email, @Message, @SubmissionDate, @CompanyId, @CreationDate, @CreatedBy);";

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
                        parameters.Add("@Email", model.Email);
                        parameters.Add("@Message", model.Message);
                        parameters.Add("@SubmissionDate", model.SubmissionDate);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@CreationDate", DateTime.UtcNow);
                        parameters.Add("@CreatedBy", GetUserName());

                        await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        transaction.Commit();
                        return (true, id, "Contact form submission added successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to add contact form submission.", ex);
                    }
                }
            }
        }

        public async Task<(bool Success, string id, string Message)> UpdateContactFormSubmissionSqlAsync(ContactFormSubmissionDto model)
        {
            const string sql = @"
            UPDATE ContactFormSubmissions
            SET Name = @Name,
                Email = @Email,
                Message = @Message,
                SubmissionDate = @SubmissionDate,
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
                        parameters.Add("@Email", model.Email);
                        parameters.Add("@Message", model.Message);
                        parameters.Add("@SubmissionDate", model.SubmissionDate);
                        parameters.Add("@CompanyId", model.CompanyId);
                        parameters.Add("@UpdateDate", DateTime.UtcNow);
                        parameters.Add("@UpdatedBy", GetUserName());

                        var rowsAffected = await connection.ExecuteAsync(sql, parameters, transaction: transaction);

                        if (rowsAffected == 0)
                        {
                            throw new NotFoundException("Contact form submission not found.");
                        }

                        transaction.Commit();
                        return (true, model.Id, "Contact form submission updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Failed to update contact form submission.", ex);
                    }
                }
            }
        }

        public async Task<IEnumerable<ContactFormSubmission>> GetContactFormSubmissionSqlAsync()
        {
            const string sql = @"
            SELECT
                cfs.Id, cfs.Name, cfs.Email, cfs.Message, cfs.SubmissionDate, cfs.CompanyId, cfs.CreationDate , cfs.CreatedBy , cfs.UpdateDate , cfs.UpdatedBy,
                c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website, 
                c.CreationDate , c.CreatedBy , c.UpdateDate , c.UpdatedBy
            FROM ContactFormSubmissions cfs
            INNER JOIN Companys c ON cfs.CompanyId = c.Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<ContactFormSubmission, Company, ContactFormSubmission>(
                        sql,
                        (submission, company) =>
                        {
                            submission.Company = company;
                            return submission;
                        }
                        
                    );

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve contact form submissions.", ex);
                }
            }
        }

        public async Task<ContactFormSubmission> GetContactFormSubmissionByIdSqlAsync(string id)
        {
            const string sql = @"
            SELECT
                cfs.Id, cfs.Name, cfs.Email, cfs.Message, cfs.SubmissionDate, cfs.CompanyId,
                 cfs.CreationDate , cfs.CreatedBy , cfs.UpdateDate , cfs.UpdatedBy,
                c.Id, c.Name, c.Description, c.EstablishedDate, c.ContactEmail, c.Phone, c.Address, c.Website, 
                    c.CreationDate , c.CreatedBy , c.UpdateDate , c.UpdatedBy
            FROM ContactFormSubmissions cfs
            INNER JOIN Companys c ON cfs.CompanyId = c.Id
            WHERE cfs.Id = @Id;";

            using (var connection = _dapperDbContext.CreateConnection())
            {
                connection.Open();
                try
                {
                    var result = await connection.QueryAsync<ContactFormSubmission, Company, ContactFormSubmission>(
                        sql,
                        (submission, company) =>
                        {
                            submission.Company = company;
                            return submission;
                        },
                        param: new { Id = id }
                        
                    );

                    return result.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to retrieve contact form submission by ID.", ex);
                }
            }
        }

    }
}
