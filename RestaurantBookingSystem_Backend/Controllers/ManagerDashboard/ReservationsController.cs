using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationsService _service;

        public ReservationsController(IReservationsService service)
        {
            _service = service;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetReservationsByRestaurant(int restaurantId, [FromQuery] int days = 30)
        {
            var reservations = await _service.GetReservationsByRestaurantIdAsync(restaurantId, days);
            return Ok(reservations);
        }

        [HttpGet("today/{restaurantId}")]
        public async Task<IActionResult> GetTodayReservations(int restaurantId)
        {
            var reservations = await _service.GetTodayReservationsAsync(restaurantId);
            return Ok(reservations);
        }

        [HttpGet("upcoming/{restaurantId}")]
        public async Task<IActionResult> GetUpcomingReservations(int restaurantId, [FromQuery] int days = 7)
        {
            var reservations = await _service.GetUpcomingReservationsAsync(restaurantId, days);
            return Ok(reservations);
        }

        [HttpGet("summary/{restaurantId}")]
        public async Task<IActionResult> GetReservationSummary(int restaurantId)
        {
            var summary = await _service.GetReservationSummaryAsync(restaurantId);
            return Ok(summary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateReservationStatus(int id, [FromBody] ReservationStatus status)
        {
            var success = await _service.UpdateStatusAsync(id, status);
            if (!success) return NotFound();
            return Ok(new { message = "Reservation status updated successfully" });
        }
    }
}