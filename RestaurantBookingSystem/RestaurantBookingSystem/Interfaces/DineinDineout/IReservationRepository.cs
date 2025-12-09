using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<List<Reservation>> GetUserReservationsAsync(int userId);
        Task<Reservation> UpdateReservationAsync(Reservation reservation);
        Task<bool> CancelReservationAsync(int reservationId);
        Task<List<Reservation>> GetTableReservationsByDateTimeAsync(int tableId, DateTime date, TimeSpan inTime, TimeSpan outTime);
        Task<List<Reservation>> GetRestaurantReservationsByDateTimeAsync(int restaurantId, DateTime date, TimeSpan inTime, TimeSpan outTime);
        Task<ReservationResponseDto> MapToResponseDtoAsync(Reservation reservation);
    }
}