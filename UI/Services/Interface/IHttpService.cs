using UI.Common;

namespace UI.Services.Interface
{
    public interface IHttpService
    {
        Task<string> SendData(ClientRequest requestData, bool token = true);
    }
}
