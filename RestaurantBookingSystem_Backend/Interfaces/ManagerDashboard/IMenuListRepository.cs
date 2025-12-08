using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IMenuListRepository
    {
        Task<IEnumerable<MenuList>> GetByRestaurantIdAsync(int restaurantId);
        Task<MenuList?> GetByIdAsync(int id);
        Task<MenuList> AddAsync(MenuList item);
        Task UpdateAsync(MenuList item);
        Task<bool> DeleteAsync(int id);
    }
}