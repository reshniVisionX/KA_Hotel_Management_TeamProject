using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly BookingContext _context;

        public ReservationRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .ThenInclude(t => t.Restaurants)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(int userId)
        {
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .ThenInclude(t => t.Restaurants)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }



        public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
        {
            _context.Reservation.Update(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            if (reservation == null) return false;

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reservation>> GetTableReservationsByDateTimeAsync(int tableId, DateTime date, TimeSpan inTime, TimeSpan outTime)
        {
            return await _context.Reservation
                .Where(r => r.TableId == tableId &&
                           r.ReservationDate.Date == date.Date &&
                           r.Status == ReservationStatus.Reserved &&
                           ((r.ReservationInTime < outTime && r.ReservationOutTime > inTime)))
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetRestaurantReservationsByDateTimeAsync(int restaurantId, DateTime date, TimeSpan inTime, TimeSpan outTime)
        {
            return await _context.Reservation
                .Include(r => r.Table)
                .Include(r => r.User)
                .Where(r => r.Table.RestaurantId == restaurantId &&
                           r.ReservationDate.Date == date.Date &&
                           r.Status == ReservationStatus.Reserved &&
                           ((r.ReservationInTime < outTime && r.ReservationOutTime > inTime)))
                .ToListAsync();
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(int userId, ReservationCreateDto request)
        {
            var reservation = new Reservation
            {
                UserId = userId,
                TableId = request.TableId,
                ReservationDate = request.ReservationDate,
                ReservationInTime = request.ReservationInTime,
                ReservationOutTime = request.ReservationOutTime,
                Status = ReservationStatus.Reserved,
                ReseveredAt = DateTime.UtcNow,
                AdvancePaymentAmount = request.AdvancePaymentAmount,
                AdvancePayment = request.AdvancePayment
            };

            var createdReservation = await CreateReservationAsync(reservation);
            var reservationWithDetails = await GetReservationByIdAsync(createdReservation.ReservationId);
            return await MapToResponseDtoAsync(reservationWithDetails);
        }

        public async Task<ReservationResponseDto> MapToResponseDtoAsync(Reservation reservation)
        {
            return new ReservationResponseDto
            {
                ReservationId = reservation.ReservationId,
                UserId = reservation.UserId,
                UserName = $"{reservation.User?.FirstName} {reservation.User?.LastName}".Trim(),
                TableId = reservation.TableId,
                TableNo = reservation.Table?.TableNo ?? "",
                RestaurantId = reservation.Table?.RestaurantId ?? 0,
                RestaurantName = reservation.Table?.Restaurants?.RestaurantName ?? "",
                ReservationDate = reservation.ReservationDate,
                ReservationInTime = reservation.ReservationInTime,
                ReservationOutTime = reservation.ReservationOutTime,
                Status = reservation.Status,
                StatusText = reservation.Status.ToString(),
                ReservedAt = reservation.ReseveredAt,
                AdvancePaymentAmount = reservation.AdvancePaymentAmount,
                AdvancePayment = reservation.AdvancePayment
            };
        }
    }
}