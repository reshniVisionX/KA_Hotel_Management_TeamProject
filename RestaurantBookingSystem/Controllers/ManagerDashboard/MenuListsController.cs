using Microsoft.AspNetCore.Mvc;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;

namespace RestaurantBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuListsController : ControllerBase
    {
        private readonly IMenuListService _service;
        private readonly ILogger<MenuListsController> _logger;

        public MenuListsController(IMenuListService service, ILogger<MenuListsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<MenuListsDto>>> GetByRestaurant(int restaurantId)
        {
            var items = await _service.GetByRestaurantIdAsync(restaurantId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuListsDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MenuListsDto>> Create([FromForm] CreateMenuListsDto dto, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            
            byte[]? imageBytes = null;
            if (image != null)
            {
                if (image.Length > 5 * 1024 * 1024) 
                    return BadRequest("Image size cannot exceed 5MB");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(image.ContentType.ToLower()))
                    return BadRequest("Only JPEG, PNG, and GIF images are allowed");

                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            
            try
            {
                var created = await _service.AddAsync(dto, imageBytes);
                _logger.LogInformation("Menu item created: {ItemName} for restaurant {RestaurantId}", created.ItemName, created.RestaurantId);
                return CreatedAtAction(nameof(GetById), new { id = created.ItemId }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Failed to create menu item: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MenuListsDto>> Update(int id, [FromForm] UpdateMenuListsDto dto, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            byte[]? imageBytes = null;
            if (image != null)
            {
                if (image.Length > 5 * 1024 * 1024) 
                    return BadRequest("Image size cannot exceed 5MB");

                var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(image.ContentType.ToLower()))
                    return BadRequest("Only JPEG, PNG, and GIF images are allowed");

                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            
            try
            {
                var updated = await _service.UpdateAsync(id, dto, imageBytes);
                return updated == null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (deleted)
                _logger.LogInformation("Menu item deleted: {ItemId}", id);
            return deleted ? NoContent() : NotFound();
        }


    }
}