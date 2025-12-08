using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class UpdateRestaurantsDto
    {
        public string? RestaurantName { get; set; }
        public string? Description { get; set; }
        public RestaurantCategory? RestaurantCategory { get; set; }
        public FoodType? RestaurantType { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? ContactNo { get; set; }
        public decimal? DeliveryCharge { get; set; }
    }
}