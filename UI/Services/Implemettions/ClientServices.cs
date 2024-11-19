using Newtonsoft.Json;
using UI.ApiSettings;
using UI.Common;
using UI.Services.Interface;

public class ClientServices<T> : IClientServices<T> where T : class
{
    private readonly IHttpService _httpService;

    public ClientServices(IHttpService httpService)
    {
        _httpService = httpService;
    }

    private async Task<TResult> SendRequestAsync<TResult>(string endpoint, ApiType apiType, object data = null, bool includeToken = true)
    {
        var request = new ClientRequest
        {
            Url = ApiUrlSettings._BaseUrl + endpoint,
            ApiType = apiType,
            ContentType = ContentType.Json,
            Data = data
        };

        string response = await _httpService.SendData(request, token: includeToken);
        return JsonConvert.DeserializeObject<TResult>(response);
    }


    public async Task<Response<object>> DeleteClientAsync(string endpoint)
    {
        return  await SendRequestAsync<Response<object>>(endpoint, ApiType.Delete);
    }

    public async Task<Response<IEnumerable<T>>> GetAllClientsAsync(string endpoint)
    {
        return await SendRequestAsync<Response<IEnumerable<T>>>(endpoint, ApiType.Get);
    }

    public async Task<Response<T>> GetClientByIdAsync(string endpoint)
    {
        return await SendRequestAsync<Response<T>>(endpoint, ApiType.Get);
    }

    public async Task<Response<object>> PostClientAsync(string endpoint, T client)
    {
        return await SendRequestAsync<Response<object>>(endpoint, ApiType.Post, client);
    }

    public async Task<Response<object>> UpdateClientAsync(string endpoint, T client)
    {
        return await SendRequestAsync<Response<object>>(endpoint, ApiType.Put, client);
    }

    public async Task<Response<LoginResponse>> Login(string endpoint, T client)
    {
        return await SendRequestAsync<Response<LoginResponse>>(endpoint, ApiType.Post, client, includeToken: false);
    }
    
}
