using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Interface.IRepository
{
    public interface IUserProfileRepository
    {
        Task<List<Users>> GetAllUsersAsync();
        Task<Users?> GetUserAsync(int userId);
        Task<Users?> GetUserByEmailAsync(string email);
        Task<Users> CreateUserAsync(Users user);
        Task<bool> UpdateUserAsync(Users user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
