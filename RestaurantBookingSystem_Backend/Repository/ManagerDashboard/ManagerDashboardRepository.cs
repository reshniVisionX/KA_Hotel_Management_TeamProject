using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class ManagerDashboardRepository : IManagerDashboardRepository
    {
        private readonly BookingContext _context;

        public ManagerDashboardRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Orders>> GetTodayOrdersByManagerAsync(int managerId)
        {
            var today = DateTime.Today;
            return await _context.Orders
                .Include(o => o.Restaurant)
                .Where(o => o.Restaurant!.ManagerId == managerId && o.OrderDate.Date == today)
                .ToListAsync();
        }

        public async Task<IEnumerable<Orders>> GetOrdersByDateAndManagerAsync(int managerId, DateTime date)
        {
            return await _context.Orders
                .Include(o => o.Restaurant)
                .Where(o => o.Restaurant!.ManagerId == managerId && o.OrderDate.Date == date.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuList>> GetLowStockItemsByManagerAsync(int managerId, int threshold = 5)
        {
            return await _context.MenuList
                .Include(m => m.Restaurant)
                .Where(m => m.Restaurant!.ManagerId == managerId && m.AvailableQty < threshold)
                .ToListAsync();
        }

        public async Task<bool> UpdateMenuItemStockAsync(int itemId, int newQuantity, int managerId)
        {
            var item = await _context.MenuList
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.ItemId == itemId && m.Restaurant!.ManagerId == managerId);
            
            if (item == null) return false;

            item.AvailableQty = newQuantity;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}