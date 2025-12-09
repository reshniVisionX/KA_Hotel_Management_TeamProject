using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Services
{
    public class RestaurantsService : IRestaurantService
    {
        private readonly IRestaurantRepository _repo;

        public RestaurantsService(IRestaurantRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RestaurantsDto>> GetAllAsync()
        {
            var restaurants = await _repo.GetAllAsync();
            return restaurants.Select(MapToDto);
        }

        public async Task<RestaurantsDto?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task<RestaurantsDto?> UpdateAsync(int id, UpdateRestaurantsDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Only update fields that are provided (not null)
            if (!string.IsNullOrEmpty(dto.RestaurantName))
                existing.RestaurantName = dto.RestaurantName;
            
            if (!string.IsNullOrEmpty(dto.Description))
                existing.Description = dto.Description;
            
            if (dto.RestaurantCategory.HasValue)
                existing.RestaurantCategory = dto.RestaurantCategory.Value;
            
            if (dto.RestaurantType.HasValue)
                existing.RestaurantType = dto.RestaurantType.Value;
            
            if (!string.IsNullOrEmpty(dto.Location))
                existing.Location = dto.Location;
            
            if (!string.IsNullOrEmpty(dto.City))
                existing.City = dto.City;
            
            if (!string.IsNullOrEmpty(dto.ContactNo))
                existing.ContactNo = dto.ContactNo;
            
            if (dto.DeliveryCharge.HasValue)
                existing.DeliveryCharge = dto.DeliveryCharge.Value;

            await _repo.UpdateAsync(existing);
            return MapToDto(existing);
        }

        public async Task<bool> UpdateStatusAsync(int id, bool isActive)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return false;

            existing.IsActive = isActive;
            await _repo.UpdateAsync(existing);
            return true;
        }



        private static RestaurantsDto MapToDto(Restaurants r)
        {
            return new RestaurantsDto
            {
                RestaurantId = r.RestaurantId,
                RestaurantName = r.RestaurantName,
                Images = r.Images,
                Description = r.Description,
                Ratings = r.Ratings,
                RestaurantCategory = r.RestaurantCategory,
                RestaurantType = r.RestaurantType,
                Location = r.Location,
                City = r.City,
                ContactNo = r.ContactNo,
                DeliveryCharge = r.DeliveryCharge,
                ManagerId = r.ManagerId,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt
            };
        }
    }
}