using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class UpdateUserProfileDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? FirstName { get; set; }
        
        [StringLength(100, MinimumLength = 2)]
        public string? LastName { get; set; }
        
        [Phone]
        public string? Mobile { get; set; }
        
        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }
    }
}
