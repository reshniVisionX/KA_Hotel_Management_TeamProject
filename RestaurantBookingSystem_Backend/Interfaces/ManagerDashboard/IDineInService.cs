using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IDineInService
    {
        Task<IEnumerable<DineInDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<DineInDto?> GetByIdAsync(int id);
        Task<DineInDto> AddAsync(CreateDineInDto dto);
        Task<DineInDto?> UpdateAsync(int id, UpdateDineInDto dto);
        Task<bool> DeleteAsync(int id);
    }
}