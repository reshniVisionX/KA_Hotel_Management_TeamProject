using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly BookingContext _context;

        public ReservationsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByRestaurantIdAsync(int restaurantId, int days = 30)
        {
            var startDate = DateTime.Today.AddDays(-days);
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.Table!.RestaurantId == restaurantId && r.ReservationDate.Date >= startDate)
                .OrderByDescending(r => r.ReservationDate)
                .ThenByDescending(r => r.ReservationInTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetTodayReservationsAsync(int restaurantId)
        {
            var today = DateTime.Today;
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.Table!.RestaurantId == restaurantId && r.ReservationDate.Date == today)
                .OrderBy(r => r.ReservationInTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetUpcomingReservationsAsync(int restaurantId, int days = 7)
        {
            var today = DateTime.Today;
            var endDate = today.AddDays(days);
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .Where(r => r.Table!.RestaurantId == restaurantId && 
                           r.ReservationDate.Date >= today && 
                           r.ReservationDate.Date <= endDate &&
                           r.Status == ReservationStatus.Reserved)
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.ReservationInTime)
                .ToListAsync();
        }

        public async Task<object> GetReservationSummaryAsync(int restaurantId)
        {
            var today = DateTime.Today;
            
            var totalReservations = await _context.Reservation
                .CountAsync(r => r.Table!.RestaurantId == restaurantId);

            var todayReservations = await _context.Reservation
                .CountAsync(r => r.Table!.RestaurantId == restaurantId && r.ReservationDate.Date == today);

            var upcomingReservations = await _context.Reservation
                .CountAsync(r => r.Table!.RestaurantId == restaurantId && 
                               r.ReservationDate.Date > today && 
                               r.Status == ReservationStatus.Reserved);

            var cancelledReservations = await _context.Reservation
                .CountAsync(r => r.Table!.RestaurantId == restaurantId && r.Status == ReservationStatus.Cancelled);

            return new
            {
                TotalReservations = totalReservations,
                TodayReservations = todayReservations,
                UpcomingReservations = upcomingReservations,
                CancelledReservations = cancelledReservations
            };
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservation
                .Include(r => r.User)
                .Include(r => r.Table)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
        }

        public async Task<bool> UpdateStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null) return false;

            reservation.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}