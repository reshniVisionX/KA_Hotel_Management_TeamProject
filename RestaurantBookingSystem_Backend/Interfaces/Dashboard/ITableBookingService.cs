using RestaurantBookingSystem.DTO;

namespace RestaurantBookingSystem.Interface.IService
{
    public interface ITableBookingService
    {
        Task<List<TableBookingDto>> GetBookingsByUserIdAsync(int userId);
        Task<List<TableBookingDto>> GetBookingsByRestaurantIdAsync(int restaurantId);
    }
}