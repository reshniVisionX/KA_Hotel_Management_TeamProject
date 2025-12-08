namespace RestaurantBookingSystem.DTO
{
    public class UserPreferenceDto
    {
        public int PreferenceId { get; set; }
        public int UserId { get; set; }
        public string? Theme { get; set; }
        public bool? NotificationsEnabled { get; set; }
        public string? PreferredCity { get; set; }
        public string? PreferredFoodType { get; set; }
    }
}
