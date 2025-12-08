using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using System;

namespace RestaurantBookingSystem.Repository
{
    public class RestaurantRepository : IRestaurant
    {
        private readonly BookingContext _context;

        public RestaurantRepository(BookingContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Restaurants>> GetAllRestaurants()
        {
            return await _context.Restaurants
                                .Where(r => r.IsActive)
                                .Include(r => r.MenuLists)
                                .ToListAsync();
        }
        public async Task<IEnumerable<MenuList>> GetMenuByRestaurantId(int restaurantId)
        {
            return await _context.MenuList
                                   .Where(m => m.RestaurantId == restaurantId)
                                   .ToListAsync();
        }

        public async  Task<Restaurants> GetRestaurantById(int id)
        {
            var restaurant = await _context.Restaurants
                                      .Include(r => r.MenuLists)
                                      .FirstOrDefaultAsync(r => r.RestaurantId == id && r.IsActive);
            if (restaurant == null)
                throw new KeyNotFoundException($"Restaurant with Id {id} not found");
            return restaurant;
        }

        public async Task<IEnumerable<Restaurants>> SearchRestaurantsAsync(string? name, string? city)
        {
            var query = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.RestaurantName.Contains(name));

            if (!string.IsNullOrEmpty(city))
                query = query.Where(r => r.City.ToLower().Contains(city.ToLower()));

            return await query.ToListAsync();
        }

        public async Task<bool> RestaurantExistsAsync(int restaurantId)
        {
            return await _context.Restaurants.AnyAsync(r => r.RestaurantId == restaurantId);
        }
    }
}
