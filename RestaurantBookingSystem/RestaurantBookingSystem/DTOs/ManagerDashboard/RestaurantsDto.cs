using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class RestaurantsDto
    {
        public int RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public byte[]? Images { get; set; }
        public string? Description { get; set; }
        public decimal? Ratings { get; set; }
        public RestaurantCategory RestaurantCategory { get; set; }
        public FoodType RestaurantType { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? ContactNo { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public int ManagerId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
       
    }
}