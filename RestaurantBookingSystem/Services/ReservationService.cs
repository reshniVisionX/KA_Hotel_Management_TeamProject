using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public ReservationService(IReservationRepository repository, ITableRepository tableRepository, IMapper mapper)
        {
            _repository = repository;
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(int userId, ReservationCreateDto request)
        {
            // Check for existing reservations at the same table, date and overlapping time
            var existingReservations = await _repository.GetTableReservationsByDateTimeAsync(
                request.TableId, request.ReservationDate, request.ReservationInTime, request.ReservationOutTime);

            if (existingReservations.Any())
            {
                throw new InvalidOperationException("Table is already booked for the selected date and time");
            }

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

            var createdReservation = await _repository.CreateReservationAsync(reservation);
            var reservationWithDetails = await _repository.GetReservationByIdAsync(createdReservation.ReservationId);
            return await _repository.MapToResponseDtoAsync(reservationWithDetails);
        }



        public async Task<List<ReservationResponseDto>> GetUserReservationsAsync(int userId)
        {
            var reservations = await _repository.GetUserReservationsAsync(userId);
            var result = new List<ReservationResponseDto>();

            foreach (var reservation in reservations)
            {
                result.Add(await _repository.MapToResponseDtoAsync(reservation));
            }

            return result;
        }



        public async Task<ReservationResponseDto?> UpdateReservationAsync(int reservationId, ReservationUpdateDto request)
        {
            var reservation = await _repository.GetReservationByIdAsync(reservationId);
            if (reservation == null) return null;

            // Check for conflicts with other reservations (excluding current one)
            var existingReservations = await _repository.GetTableReservationsByDateTimeAsync(
                reservation.TableId, request.ReservationDate, request.ReservationInTime, request.ReservationOutTime);

            var hasConflict = existingReservations.Any(r => r.ReservationId != reservationId);
            if (hasConflict)
            {
                throw new InvalidOperationException("Table is already booked for the selected date and time");
            }

            reservation.ReservationDate = request.ReservationDate;
            reservation.ReservationInTime = request.ReservationInTime;
            reservation.ReservationOutTime = request.ReservationOutTime;
            reservation.AdvancePaymentAmount = request.AdvancePaymentAmount;
            reservation.AdvancePayment = request.AdvancePayment;

            await _repository.UpdateReservationAsync(reservation);
            var updatedReservationWithDetails = await _repository.GetReservationByIdAsync(reservationId);
            return await _repository.MapToResponseDtoAsync(updatedReservationWithDetails);
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            return await _repository.CancelReservationAsync(reservationId);
        }

        public async Task<RestaurantAvailabilityDto> CheckAvailabilityAsync(BookingRequestDto request)
        {
            var timeSlots = new List<TimeSlotAvailabilityDto>();
            var standardSlots = new List<(TimeSpan Start, TimeSpan End)>
            {
                (new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0)),
                (new TimeSpan(13, 30, 0), new TimeSpan(15, 30, 0)),
                (new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0)),
                (new TimeSpan(20, 30, 0), new TimeSpan(22, 30, 0)),
                (new TimeSpan(19, 0, 0), new TimeSpan(21, 0, 0))
            };

            // Get all restaurant tables to calculate total capacity
            var allTables = await GetRestaurantTablesAsync(request.RestaurantId);
            var totalRestaurantCapacity = allTables.Sum(t => t.Capacity);

            foreach (var slot in standardSlots)
            {
                var existingReservations = await _repository.GetRestaurantReservationsByDateTimeAsync(
                    request.RestaurantId, request.BookingDate, slot.Start, slot.End);

                var bookedCapacity = existingReservations.Sum(r => r.Table?.Capacity ?? 0);
                var availableCapacity = totalRestaurantCapacity - bookedCapacity;

                timeSlots.Add(new TimeSlotAvailabilityDto
                {
                    TimeSlot = $"{slot.Start:hh\\:mm}-{slot.End:hh\\:mm}",
                    StartTime = slot.Start,
                    EndTime = slot.End,
                    IsAvailable = availableCapacity >= request.GuestCount,
                    AvailableCapacity = availableCapacity,
                    TotalCapacity = totalRestaurantCapacity
                });
            }

            return new RestaurantAvailabilityDto
            {
                RestaurantId = request.RestaurantId,
                RestaurantName = allTables.FirstOrDefault()?.Restaurants?.RestaurantName ?? "",
                BookingDate = request.BookingDate,
                RequestedGuests = request.GuestCount,
                TimeSlots = timeSlots,
                TotalRestaurantCapacity = totalRestaurantCapacity
            };
        }

        public async Task<ReservationResponseDto> CreateSmartBookingAsync(int userId, SmartBookingRequestDto request)
        {
            var timeParts = request.TimeSlot.Split('-');
            var startTime = TimeSpan.Parse(timeParts[0]);
            var endTime = TimeSpan.Parse(timeParts[1]);

            // Find best available table for the guest count
            var bestTable = await FindBestAvailableTableAsync(request.RestaurantId, request.BookingDate, 
                startTime, endTime, request.GuestCount);

            if (bestTable == null)
            {
                throw new InvalidOperationException("No suitable table available for the requested time and guest count");
            }

            var reservationRequest = new ReservationCreateDto
            {
                TableId = bestTable.TableId,
                ReservationDate = request.BookingDate,
                ReservationInTime = startTime,
                ReservationOutTime = endTime,
                AdvancePaymentAmount = request.AdvancePaymentAmount,
                AdvancePayment = request.AdvancePayment
            };

            return await CreateReservationAsync(userId, reservationRequest);
        }

        public async Task<List<TimeSlotAvailabilityDto>> GetTimeSlotsAsync(int restaurantId, DateTime date, int guestCount)
        {
            var request = new BookingRequestDto
            {
                RestaurantId = restaurantId,
                BookingDate = date,
                GuestCount = guestCount
            };

            var availability = await CheckAvailabilityAsync(request);
            return availability.TimeSlots.Where(ts => ts.IsAvailable).ToList();
        }

        private async Task<List<Model.Restaurant.DineIn>> GetRestaurantTablesAsync(int restaurantId)
        {
            return await _tableRepository.GetRestaurantTablesAsync(restaurantId);
        }

        private async Task<Model.Restaurant.DineIn?> FindBestAvailableTableAsync(int restaurantId, DateTime date, 
            TimeSpan startTime, TimeSpan endTime, int guestCount)
        {
            var allTables = await GetRestaurantTablesAsync(restaurantId);
            var existingReservations = await _repository.GetRestaurantReservationsByDateTimeAsync(
                restaurantId, date, startTime, endTime);

            var bookedTableIds = existingReservations.Select(r => r.TableId).ToHashSet();

            // Find available tables that can accommodate guests
            var suitableTables = allTables
                .Where(t => !bookedTableIds.Contains(t.TableId) && 
                           t.Status == TableStatus.Available && 
                           t.Capacity >= guestCount)
                .OrderBy(t => t.Capacity) // Prefer smaller tables that fit
                .ToList();

            return suitableTables.FirstOrDefault();
        }
    }
}