namespace COMERP.Abstractions.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        Task<(bool Success, string Message)> AddAsync(T entity);
        Task<(bool Success, string Message)> UpdateAsync(T entity);
        Task<(bool Success, string Message)> DeleteAsync(string id);
        Task<(bool Success, string id, string Message)> AddSqlAsync(T entity);
        Task<(bool Success, string id, string Message)> UpdateSqlAsync(T entity);
        Task<(bool Success, string id, string Message)> DeleteSqlAsync(string id);
        Task<IEnumerable<T>> GetAllSqlAsync();
        Task<T> GetByIdSqlAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
    }
}
