using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ITableService
    {
        Task<List<TableResponseDto>> GetAvailableTablesAsync(int restaurantId);
    }
}