using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Repository
{
    public class Restaurants_Repository : IRestaurantsRepository
    {
        private readonly BookingContext _context;

        public Restaurants_Repository(BookingContext context)
        {
            _context = context;
        }

        public async Task<Restaurants> CreateRestaurantAsync(Restaurants restaurant)
        {
            try
            {
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                return restaurant;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                    ex.InnerException?.Message.Contains("duplicate key") == true)
                {
                    throw new ArgumentException("Restaurant name already exists in this city");
                }
                if (ex.InnerException?.Message.Contains("FOREIGN KEY constraint failed") == true ||
                    ex.InnerException?.Message.Contains("foreign key constraint") == true)
                {
                    throw new ArgumentException("Invalid Manager ID");
                }
                throw;
            }
        }

        public async Task<Restaurants?> GetRestaurantByNameAndCityAsync(string name, string city)
        {
            return await _context.Restaurants
                .FirstOrDefaultAsync(r => r.RestaurantName == name && r.City == city);
        }
    }
}
