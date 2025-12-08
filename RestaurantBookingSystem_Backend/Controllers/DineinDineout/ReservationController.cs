using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult> GetUserReservations(int userId)
        {
            var result = await _service.GetUserReservationsAsync(userId);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "User reservations retrieved successfully",
                Data = result
            });
        }

        [HttpPut("update/{reservationId}")]
        public async Task<ActionResult> UpdateReservation(int reservationId, [FromBody] ReservationUpdateDto request)
        {
            var result = await _service.UpdateReservationAsync(reservationId, request);
            if (result == null) throw new AppException("Reservation not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Reservation updated successfully",
                Data = result
            });
        }

        [HttpPatch("cancel/{reservationId}")]
        public async Task<ActionResult> CancelReservation(int reservationId)
        {
            var result = await _service.CancelReservationAsync(reservationId);
            if (!result) throw new AppException("Reservation not found");
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Reservation cancelled successfully",
                Data = null
            });
        }

        [HttpPost("check-availability")]
        public async Task<ActionResult> CheckAvailability([FromBody] BookingRequestDto request)
        {
            var result = await _service.CheckAvailabilityAsync(request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Availability checked successfully",
                Data = result
            });
        }

        [HttpPost("smart-booking/{userId}")]
        public async Task<ActionResult> CreateSmartBooking(int userId, [FromBody] SmartBookingRequestDto request)
        {
            var result = await _service.CreateSmartBookingAsync(userId, request);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Smart booking created successfully",
                Data = result
            });
        }

        [HttpGet("time-slots/{restaurantId}")]
        public async Task<ActionResult> GetTimeSlots(int restaurantId, [FromQuery] DateTime date, [FromQuery] int guestCount)
        {
            var result = await _service.GetTimeSlotsAsync(restaurantId, date, guestCount);
            return Ok(new ApiSuccessResponse<object>
            {
                Message = "Time slots retrieved successfully",
                Data = result
            });
        }
    }
}