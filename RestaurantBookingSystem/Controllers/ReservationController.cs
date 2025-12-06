using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<ReservationResponseDto>>> GetUserReservations(int userId)
        {
            var result = await _service.GetUserReservationsAsync(userId);
            return Ok(result);
        }



        [HttpPut("update/{reservationId}")]
        public async Task<ActionResult<ReservationResponseDto>> UpdateReservation(int reservationId, [FromBody] ReservationUpdateDto request)
        {
            var result = await _service.UpdateReservationAsync(reservationId, request);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPatch("cancel/{reservationId}")]
        public async Task<ActionResult> CancelReservation(int reservationId)
        {
            var result = await _service.CancelReservationAsync(reservationId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("check-availability")]
        public async Task<ActionResult<RestaurantAvailabilityDto>> CheckAvailability([FromBody] BookingRequestDto request)
        {
            var result = await _service.CheckAvailabilityAsync(request);
            return Ok(result);
        }

        [HttpPost("smart-booking/{userId}")]
        public async Task<ActionResult<ReservationResponseDto>> CreateSmartBooking(int userId, [FromBody] SmartBookingRequestDto request)
        {
            var result = await _service.CreateSmartBookingAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("time-slots/{restaurantId}")]
        public async Task<ActionResult<List<TimeSlotAvailabilityDto>>> GetTimeSlots(int restaurantId, [FromQuery] DateTime date, [FromQuery] int guestCount)
        {
            var result = await _service.GetTimeSlotsAsync(restaurantId, date, guestCount);
            return Ok(result);
        }
    }
}