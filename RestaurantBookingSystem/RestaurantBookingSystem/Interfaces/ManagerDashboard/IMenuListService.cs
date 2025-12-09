using RestaurantBookingSystem.Dtos;

namespace RestaurantBookingSystem.Interface
{
    public interface IMenuListService
    {
        Task<IEnumerable<MenuListsDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<MenuListsDto?> GetByIdAsync(int id);
        Task<MenuListsDto> AddAsync(CreateMenuListsDto dto, byte[]? imageBytes = null);
        Task<MenuListsDto?> UpdateAsync(int id, UpdateMenuListsDto dto);
        Task<bool> DeleteAsync(int id);
    }
}