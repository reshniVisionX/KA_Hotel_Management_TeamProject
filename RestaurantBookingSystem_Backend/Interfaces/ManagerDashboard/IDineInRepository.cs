using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Interface
{
    public interface IDineInRepository
    {
        Task<IEnumerable<DineIn>> GetByRestaurantIdAsync(int restaurantId);
        Task<DineIn?> GetByIdAsync(int id);
        Task<DineIn> AddAsync(DineIn table);
        Task<DineIn> UpdateAsync(DineIn table);
        Task<bool> DeleteAsync(int id);
    }
}