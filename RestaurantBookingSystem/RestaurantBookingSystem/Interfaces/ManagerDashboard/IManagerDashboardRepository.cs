using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IManagerDashboardRepository
    {
        Task<IEnumerable<Orders>> GetTodayOrdersByManagerAsync(int managerId);
        Task<IEnumerable<Orders>> GetOrdersByDateAndManagerAsync(int managerId, DateTime date);
        Task<IEnumerable<MenuList>> GetLowStockItemsByManagerAsync(int managerId, int threshold = 5);
        Task<bool> UpdateMenuItemStockAsync(int itemId, int newQuantity, int managerId);
    }
}