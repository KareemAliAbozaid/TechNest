namespace TechNest.API.APIResponse
{
    public class APIErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }


        public APIErrorResponse(int Number, string? ErrorMessage = null)
        {
            StatusCode = Number;
            Message = !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : GetDefaultErrorMessage(Number);
        }


        public string GetDefaultErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Resource Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => "An error occurred"
            };
        }

    }
}
