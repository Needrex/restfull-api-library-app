namespace RestApiApp.Utils
{
    public class ApiRespone<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiRespone(bool success, string message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}