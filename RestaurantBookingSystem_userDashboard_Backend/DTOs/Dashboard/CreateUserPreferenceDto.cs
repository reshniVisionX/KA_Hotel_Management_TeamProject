using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class CreateUserPreferenceDto
    {
        [Required]
        public int UserId { get; set; }

        public string? Theme { get; set; } = "Light";
        public bool NotificationsEnabled { get; set; } = true;
        public string? PreferredCity { get; set; }
        public string? PreferredFoodType { get; set; }
    }
}