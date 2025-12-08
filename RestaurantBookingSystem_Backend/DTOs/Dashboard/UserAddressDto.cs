namespace RestaurantBookingSystem.DTO
{
    public class UserAddressDto
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string? Landmark { get; set; }
        public string ContactNo { get; set; }
        public bool IsDefault { get; set; }
    }
}