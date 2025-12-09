namespace RestaurantBookingSystem.DTO
{
    public class CreateRestaurantDto
    {
        public string RestaurantName { get; set; }
        public string? Description { get; set; }
        public string RestaurantCategory { get; set; } = "Restaurant";
        public string RestaurantType { get; set; } = "Both";
        public string Location { get; set; }
        public string City { get; set; }
        public string ContactNo { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public int ManagerId { get; set; }
    }
}
