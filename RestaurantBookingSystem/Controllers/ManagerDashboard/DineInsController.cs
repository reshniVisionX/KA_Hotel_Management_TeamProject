using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DineInsController : ControllerBase
    {
        private readonly IDineInService _service;
        private readonly ILogger<DineInsController> _logger;

        public DineInsController(IDineInService service, ILogger<DineInsController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<DineInDto>>> GetByRestaurant(int restaurantId)
        {
            var tables = await _service.GetByRestaurantIdAsync(restaurantId);
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DineInDto>> GetById(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid table ID");
                var table = await _service.GetByIdAsync(id);
                return table == null ? NotFound() : Ok(table);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById failed for ID: {Id}", id);
                return StatusCode(500, "Error retrieving table");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DineInDto>> Create([FromBody] CreateDineInDto dto)
        {
            try
            {
                if (dto == null || !ModelState.IsValid) return BadRequest(ModelState);
                var created = await _service.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.TableId }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Create table failed");
                return StatusCode(500, "Error creating table");
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<DineInDto>> Update(int id, [FromBody] UpdateDineInDto dto)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid table ID");
                if (dto == null) return BadRequest("Request body required");
                
                var updated = await _service.UpdateAsync(id, dto);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update table failed for ID: {Id}", id);
                return StatusCode(500, "Error updating table");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Invalid table ID");
                var deleted = await _service.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete table failed for ID: {Id}", id);
                return StatusCode(500, "Error deleting table");
            }
        }
    }
}