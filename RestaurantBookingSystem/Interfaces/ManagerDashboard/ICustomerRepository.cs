using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Users>> GetCustomersByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Users>> GetRecentCustomersAsync(int restaurantId, int days = 30);
        Task<IEnumerable<Users>> GetFrequentCustomersAsync(int restaurantId, int minOrders = 3);
        Task<object> GetCustomerSummaryAsync(int restaurantId);
    }
}