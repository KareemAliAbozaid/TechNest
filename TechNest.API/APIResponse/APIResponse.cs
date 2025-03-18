namespace TechNest.API.APIResponse
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }

        public APIResponse()
        {
            Timestamp = DateTime.Now;
        }

        public APIResponse(T data, string message = null, int statusCode = 200)
        {
            Success = true;
            Message = message ?? "Operation completed successfully";
            Data = data;
            StatusCode = statusCode;
            Timestamp = DateTime.Now;
        }
    }
}
