using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOs
{
    public class ManagerRestaurantRequestDTO
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public IsVerified? Verification { get; set; } = IsVerified.Unverified;

        public ICollection<Restaurants>? Restaurants { get; set; }

    }
}
