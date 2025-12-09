using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class MenuListRepository : IMenuList
    {
        private readonly BookingContext _context;

        public MenuListRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuList>> GetMenuByRestaurantId(int restaurantId)
        {
            return await _context.MenuList
                                 .Where(m => m.RestaurantId == restaurantId)
                                 .ToListAsync();
        }

        public async Task<MenuList> GetMenuItemById(int id)
        {
            var item = await _context.MenuList.FindAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"Menu item with Id {id} not found");
            return item;
        }

        public async Task<IEnumerable<MenuList>> SearchMenuByName(string itemName)
        {
            return await _context.MenuList
                                 .Include(m => m.Restaurant)
                                 .Where(m => m.ItemName.ToLower().Contains(itemName.ToLower()))
                                 .ToListAsync();
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }
    }
}
