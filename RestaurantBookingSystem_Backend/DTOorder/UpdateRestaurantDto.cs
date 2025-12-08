namespace RestaurantBookingSystem.DTOorder
{
    public class UpdateRestaurantDto
    {
        public string? RestaurantName { get; set; }
        public string? Description { get; set; }
        public string? RestaurantCategory { get; set; }
        public string? RestaurantType { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? ContactNo { get; set; }
        public decimal? DeliveryCharge { get; set; }
    }
}
