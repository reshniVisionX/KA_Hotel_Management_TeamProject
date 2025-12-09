using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByRestaurantAsync(int restaurantId)
        {
            var customers = await _repo.GetCustomersByRestaurantIdAsync(restaurantId);
            return customers.Select(MapToDto);
        }

        public async Task<IEnumerable<CustomerDto>> GetRecentCustomersAsync(int restaurantId, int days = 30)
        {
            var customers = await _repo.GetRecentCustomersAsync(restaurantId, days);
            return customers.Select(MapToDto);
        }

        public async Task<IEnumerable<CustomerDto>> GetFrequentCustomersAsync(int restaurantId, int minOrders = 3)
        {
            var customers = await _repo.GetFrequentCustomersAsync(restaurantId, minOrders);
            return customers.Select(MapToDto);
        }
        public async Task<CustomerSummaryDto> GetCustomerSummaryAsync(int restaurantId)
        {
            var summary = await _repo.GetCustomerSummaryAsync(restaurantId);
            return new CustomerSummaryDto
            {
                TotalCustomers = (int)((dynamic)summary).TotalCustomers,
                RecentCustomers = (int)((dynamic)summary).RecentCustomers,
                FrequentCustomers = (int)((dynamic)summary).FrequentCustomers,
                NewCustomersThisMonth = (int)((dynamic)summary).NewCustomersThisMonth
            };
        }
        private static CustomerDto MapToDto(Users u)
        {
            return new CustomerDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin
            };
        }
    }
}