namespace RestaurantBookingSystem.DTOorder
{
    public class DeliveryPersonDto
    {
        public int DeliveryPersonId { get; set; }
        public string DeliveryName { get; set; }
        public string MobileNo { get; set; }
        public string? Email { get; set; }
        public string LicenseNumber { get; set; }
        public int? otp { get; set; }
        public string Status { get; set; }
        public int TotalDeliveries { get; set; }
        public double AverageRating { get; set; }
    }
}
