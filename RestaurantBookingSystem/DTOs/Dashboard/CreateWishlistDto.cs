using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class CreateWishlistDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }
    }
}