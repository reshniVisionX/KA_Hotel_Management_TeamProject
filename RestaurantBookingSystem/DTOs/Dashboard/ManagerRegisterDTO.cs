using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class ManagerRegisterDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ManagerName { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
        public int UserId { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        [StringLength(15, MinimumLength = 10)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        // Restaurant details (nested DTO)
        [Required]
        public RestaurantCreateDTO Restaurant { get; set; } = new RestaurantCreateDTO();
    }
}
