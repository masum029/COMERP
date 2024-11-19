using System.Net;
namespace COMERP.Common
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public string? id { get; set; }
    }
}
