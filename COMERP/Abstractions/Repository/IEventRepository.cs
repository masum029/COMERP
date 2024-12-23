using COMERP.Abstractions.Repository.Base;
using COMERP.DTOs;
using COMERP.Entities;

namespace COMERP.Abstractions.Repository
{
    public interface IEventRepository : IRepository<Event>
    {
        // Add specific command methods here if needed
        Task<(bool Success, string id, string Message)> AddEventSqlAsync(EventDto model);
        Task<(bool Success, string id, string Message)> UpdateEventSqlAsync(EventDto model);
        Task<IEnumerable<Event>> GetEventSqlAsync();
        Task<Event> GetEventByIdSqlAsync(string id);
    }
}
