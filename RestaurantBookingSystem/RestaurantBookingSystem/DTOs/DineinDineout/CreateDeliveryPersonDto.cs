using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class CreateDeliveryPersonDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string DeliveryName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(10)]
        public string MobileNo { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required]
        [StringLength(15)]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}