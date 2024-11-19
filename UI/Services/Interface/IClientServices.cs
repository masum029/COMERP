using UI.Common;

namespace UI.Services.Interface
{
    public interface IClientServices<T> where T : class
    {
        Task<Response<IEnumerable<T>>> GetAllClientsAsync(string endpoint);
        Task<Response<T>> GetClientByIdAsync(string endpoint);
        Task<Response<object>> PostClientAsync(string endpoint, T data);
        Task<Response<object>> UpdateClientAsync(string endpoint, T data);
        Task<Response<object>> DeleteClientAsync(string endpoint);
        Task<Response<LoginResponse>> Login(string endpoint, T data);
    }
}


