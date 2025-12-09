namespace RestaurantBookingSystem.Utils
{
        public class ApiSuccessResponse<T>
        {
            public bool Success { get; set; } = true;
            public string Message { get; set; } = "";
            public T Data { get; set; } = default!;
        }

        public class ApiErrorResponse
        {
            public bool Success { get; set; } = false;
            public string Message { get; set; } = "";
        }
    }
 