using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class MenuListsRepository : IMenuListRepository
    {
        private readonly BookingContext _context;

        public MenuListsRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<MenuList?> GetByIdAsync(int id)
        {
            return await _context.MenuList
                .FirstOrDefaultAsync(m => m.ItemId == id);
        }

        public async Task<IEnumerable<MenuList>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.MenuList
                .Where(m => m.RestaurantId == restaurantId)
                .ToListAsync();
        }

        public async Task<MenuList> AddAsync(MenuList item)
        {
            _context.MenuList.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(MenuList item)
        {
            _context.MenuList.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.MenuList
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null) return false;

            _context.MenuList.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}