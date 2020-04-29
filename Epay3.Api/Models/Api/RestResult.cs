namespace Epay3.Api.Models.Api
{
    public class RestResult<T>
    {
        public RestResult(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public RestResult(bool success, T data):this(success, null, data){}

        public RestResult(T data):this(true, null, data) {}

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static RestResult<T> FromResult(T result)
        {
            return new RestResult<T>(true,null,result);
        }
    }
}