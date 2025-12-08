using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDto>> GetCustomersByRestaurantAsync(int restaurantId);
        Task<IEnumerable<CustomerDto>> GetRecentCustomersAsync(int restaurantId, int days = 30);
        Task<IEnumerable<CustomerDto>> GetFrequentCustomersAsync(int restaurantId, int minOrders = 3);
        Task<CustomerSummaryDto> GetCustomerSummaryAsync(int restaurantId);
    }
}