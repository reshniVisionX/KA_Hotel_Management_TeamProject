using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.DTOs.Admin
{
    public class DeliveryPersonHistory
    {
        // Delivery Info
        public int? DeliveryId { get; set; }
        public DeliveryStatus? DeliveryStatus { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }

        // Delivery Address Info
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Pincode { get; set; }
        public string? Landmark { get; set; }

        // NEW: Customer Details
        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }

        // Delivery Person Info
        public int DeliveryPersonId { get; set; }
        public string DeliveryName { get; set; }
        public string LicenseNumber { get; set; }
        public int? Otp { get; set; }
        public DeliveryPersonStatus Status { get; set; }
        public int TotalDeliveries { get; set; }
        public double AverageRating { get; set; }
    }
}
