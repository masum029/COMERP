using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IClientRepository : IRepository<Client>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddClientSqlAsync(ClientDto model);
        Task<(bool Success, string id, string Message)> UpdateClientSqlAsync(ClientDto model);
        Task<IEnumerable<Client>> GetClientSqlAsync();
        Task<Client> GetClientByIdSqlAsync(string id);
    }
}
