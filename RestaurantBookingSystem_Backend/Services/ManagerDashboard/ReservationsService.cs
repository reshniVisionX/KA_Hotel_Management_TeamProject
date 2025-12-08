using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IReservationsRepository _repo;

        public ReservationsService(IReservationsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ReservationsDto>> GetReservationsByRestaurantIdAsync(int restaurantId, int days = 30)
        {
            var reservations = await _repo.GetReservationsByRestaurantIdAsync(restaurantId, days);
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationsDto>> GetTodayReservationsAsync(int restaurantId)
        {
            var reservations = await _repo.GetTodayReservationsAsync(restaurantId);
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationsDto>> GetUpcomingReservationsAsync(int restaurantId, int days = 7)
        {
            var reservations = await _repo.GetUpcomingReservationsAsync(restaurantId, days);
            return reservations.Select(MapToDto);
        }

        public async Task<object> GetReservationSummaryAsync(int restaurantId)
        {
            return await _repo.GetReservationSummaryAsync(restaurantId);
        }

        public async Task<ReservationsDto?> GetByIdAsync(int id)
        {
            var reservation = await _repo.GetByIdAsync(id);
            return reservation == null ? null : MapToDto(reservation);
        }

        public async Task<bool> UpdateStatusAsync(int id, ReservationStatus status)
        {
            return await _repo.UpdateStatusAsync(id, status);
        }

        private static ReservationsDto MapToDto(Reservation r)
        {
            return new ReservationsDto
            {
                ReservationId = r.ReservationId,
                UserId = r.UserId,
                CustomerName = $"{r.User?.FirstName} {r.User?.LastName}".Trim(),
                CustomerEmail = r.User?.Email,
                TableId = r.TableId,
                TableNo = r.Table?.TableNo,
                TableCapacity = r.Table?.Capacity ?? 0,
                ReservationDate = r.ReservationDate,
                ReservationInTime = r.ReservationInTime,
                ReservationOutTime = r.ReservationOutTime,
                Status = r.Status,
                ReseveredAt = DateTime.Now,
                AdvancePayment = r.AdvancePayment
            };
        }
    }
}