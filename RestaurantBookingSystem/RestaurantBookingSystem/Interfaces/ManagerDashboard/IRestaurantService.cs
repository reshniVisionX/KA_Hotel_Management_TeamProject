using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantsDto>> GetAllAsync();
        Task<RestaurantsDto?> GetByIdAsync(int id);
        Task<RestaurantsDto?> UpdateAsync(int id, UpdateRestaurantsDto dto);
        Task<bool> UpdateStatusAsync(int id, bool isActive);
    }
}