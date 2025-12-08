using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.DTOorder
{
    public class DeliveryResponseDto
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public string? DeliveryPersonName { get; set; }
        public string? DeliveryPersonMobile { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
    }
}
