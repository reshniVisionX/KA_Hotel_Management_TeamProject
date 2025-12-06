using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs
{
    public class WishlistCreateDto
    {
        [Required]
        public int ItemId { get; set; }
        
        [Required]
        public int RestaurantId { get; set; }
    }

    public class WishlistResponseDto
    {
        public int WishlistId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class WishlistCheckDto
    {
        public bool IsInWishlist { get; set; }
        public int? WishlistId { get; set; }
    }
}