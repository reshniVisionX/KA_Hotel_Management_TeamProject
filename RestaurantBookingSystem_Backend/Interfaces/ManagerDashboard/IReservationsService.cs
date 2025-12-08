using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IReservationsService
    {
        Task<IEnumerable<ReservationsDto>> GetReservationsByRestaurantIdAsync(int restaurantId, int days = 30);
        Task<IEnumerable<ReservationsDto>> GetTodayReservationsAsync(int restaurantId);
        Task<IEnumerable<ReservationsDto>> GetUpcomingReservationsAsync(int restaurantId, int days = 7);
        Task<object> GetReservationSummaryAsync(int restaurantId);
        Task<ReservationsDto?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, ReservationStatus status);
    }
}