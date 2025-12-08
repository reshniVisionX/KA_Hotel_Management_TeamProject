namespace RestaurantBookingSystem.DTO
{
    public class UpdateUserPreferenceDto
    {
        public string? Theme { get; set; } 
        public bool NotificationsEnabled { get; set; }
        public string? PreferredCity { get; set; }
        public string? PreferredFoodType { get; set; }
    }
}
