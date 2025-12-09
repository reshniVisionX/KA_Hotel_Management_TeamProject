namespace RestaurantBookingSystem.Utils
{

        public class AppException : Exception
        {
        public AppException(string? message) : base(message)
        {
        }

        public AppException(string message, Exception ex)
                : base(message)
            {
            }
        }
    }

