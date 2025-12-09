using Microsoft.Extensions.Logging;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Model.Customers;
using System;
using System.Threading.Tasks;

namespace RestaurantBookingSystem.Utils
{
    public class NotifyUsers
    {
        private readonly BookingContext _context;
        private readonly ILogger<NotifyUsers> _logger;

        public NotifyUsers(BookingContext context, ILogger<NotifyUsers> logger)
        {
            _context = context;
            _logger = logger;
        }

        
        public async Task<bool> SendNotificationAsync(int userId, string message)
        {
            try
            {
                // Validate inputs
                if (userId <= 0)
                {
                    _logger.LogWarning("❌ Invalid UserId provided for notification.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogWarning("⚠️ Empty message provided for notification.");
                    return false;
                }

                // Create notification entity
                var notification = new Notifications
                {
                    UserId = userId,
                    Message = message,
                    CreatedAt = DateTime.Now
                };

                // Save to DB
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                _logger.LogInformation("✅ Notification added for UserId: {UserId} | Message: {Message}", userId, message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error inserting notification for UserId: {UserId}", userId);
                return false;
            }
        }
    }
}
