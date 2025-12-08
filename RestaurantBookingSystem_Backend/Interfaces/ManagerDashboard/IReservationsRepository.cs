using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IReservationsRepository
    {
        Task<IEnumerable<Reservation>> GetReservationsByRestaurantIdAsync(int restaurantId, int days = 30);
        Task<IEnumerable<Reservation>> GetTodayReservationsAsync(int restaurantId);
        Task<IEnumerable<Reservation>> GetUpcomingReservationsAsync(int restaurantId, int days = 7);
        Task<object> GetReservationSummaryAsync(int restaurantId);
        Task<Reservation?> GetByIdAsync(int id);
        Task<bool> UpdateStatusAsync(int id, ReservationStatus status);
    }
}