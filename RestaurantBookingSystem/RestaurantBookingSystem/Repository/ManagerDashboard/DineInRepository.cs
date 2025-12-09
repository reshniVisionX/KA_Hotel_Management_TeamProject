using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class DineInRepository : IDineInRepository
    {
        private readonly BookingContext _context;

        public DineInRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<DineIn?> GetByIdAsync(int id)
        {
            return await _context.DineIn
                .FirstOrDefaultAsync(d => d.TableId == id);
        }

        public async Task<IEnumerable<DineIn>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.DineIn
                .Where(d => d.RestaurantId == restaurantId)
                .OrderBy(d => d.TableNo)
                .ToListAsync();
        }

        public async Task<IEnumerable<DineIn>> GetByRestaurantAndStatusAsync(int restaurantId, TableStatus status)
        {
            return await _context.DineIn
                .Where(d => d.RestaurantId == restaurantId && d.Status == status)
                .OrderBy(d => d.TableNo)
                .ToListAsync();
        }

        public async Task<DineIn> AddAsync(DineIn table)
        {
            _context.DineIn.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<DineIn> UpdateAsync(DineIn table)
        {
            _context.DineIn.Update(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.DineIn
                .FirstOrDefaultAsync(d => d.TableId == id);
            if (existing == null) return false;

            _context.DineIn.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}