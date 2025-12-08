namespace RestaurantBookingSystem.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(bool success, string message, T data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>(false, message);
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse(bool success, string message) : base(success, message) { }

        public static ApiResponse Success(string message = "Success")
        {
            return new ApiResponse(true, message);
        }

        public static ApiResponse Error(string message)
        {
            return new ApiResponse(false, message);
        }
    }
}