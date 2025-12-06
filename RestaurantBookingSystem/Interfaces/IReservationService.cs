using RestaurantBookingSystem.DTOs;

namespace RestaurantBookingSystem.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(int userId, ReservationCreateDto request);
        Task<List<ReservationResponseDto>> GetUserReservationsAsync(int userId);
        Task<ReservationResponseDto?> UpdateReservationAsync(int reservationId, ReservationUpdateDto request);
        Task<bool> CancelReservationAsync(int reservationId);
        Task<RestaurantAvailabilityDto> CheckAvailabilityAsync(BookingRequestDto request);
        Task<ReservationResponseDto> CreateSmartBookingAsync(int userId, SmartBookingRequestDto request);
        Task<List<TimeSlotAvailabilityDto>> GetTimeSlotsAsync(int restaurantId, DateTime date, int guestCount);
    }
}