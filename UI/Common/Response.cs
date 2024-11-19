namespace UI.Common
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; } = false;
        public string Detail { get; set; }
    }
}
