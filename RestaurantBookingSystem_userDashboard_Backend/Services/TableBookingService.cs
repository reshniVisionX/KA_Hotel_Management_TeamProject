using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;

namespace RestaurantBookingSystem.Services
{
    public class TableBookingService : ITableBookingService
    {
        private readonly ITableBookingRepository _tableBookingRepo;

        public TableBookingService(ITableBookingRepository tableBookingRepo)
        {
            _tableBookingRepo = tableBookingRepo;
        }

        public async Task<List<TableBookingDto>> GetBookingsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            var bookings = await _tableBookingRepo.GetBookingsByUserIdAsync(userId);
            return bookings.Select(b => new TableBookingDto
            {
                ReservationId = b.ReservationId,
                UserId = b.UserId,
                TableId = b.TableId,
                ReservationDate = b.ReservationDate,
                ReservationInTime = b.ReservationInTime,
                ReservationOutTime = b.ReservationOutTime,
                Status = b.Status,
                ReseveredAt = b.ReseveredAt,
                AdvancePayment = b.AdvancePayment,
                UserName = b.User != null ? $"{b.User.FirstName} {b.User.LastName}" : null,
                TableNo = b.Table?.TableNo,
                RestaurantId = b.Table?.RestaurantId,
                RestaurantName = b.Table?.Restaurants?.RestaurantName
            }).ToList();
        }

        public async Task<List<TableBookingDto>> GetBookingsByRestaurantIdAsync(int restaurantId)
        {
            if (restaurantId <= 0)
                throw new ArgumentException("Restaurant ID must be greater than 0");

            var bookings = await _tableBookingRepo.GetBookingsByRestaurantIdAsync(restaurantId);
            return bookings.Select(b => new TableBookingDto
            {
                ReservationId = b.ReservationId,
                UserId = b.UserId,
                TableId = b.TableId,
                ReservationDate = b.ReservationDate,
                ReservationInTime = b.ReservationInTime,
                ReservationOutTime = b.ReservationOutTime,
                Status = b.Status,
                ReseveredAt = b.ReseveredAt,
                AdvancePayment = b.AdvancePayment,
                UserName = b.User != null ? $"{b.User.FirstName} {b.User.LastName}" : null,
                TableNo = b.Table?.TableNo,
                RestaurantId = b.Table?.RestaurantId,
                RestaurantName = b.Table?.Restaurants?.RestaurantName
            }).ToList();
        }
    }
}