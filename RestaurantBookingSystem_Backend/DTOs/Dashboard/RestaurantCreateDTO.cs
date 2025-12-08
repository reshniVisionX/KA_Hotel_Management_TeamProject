using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class RestaurantCreateDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string RestaurantName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        [StringLength(15, MinimumLength = 10)]
        public string ContactNo { get; set; } = string.Empty;
        
        [Range(0, 999.99)]
        public decimal? DeliveryCharge { get; set; }
        
        [Required]
        public FoodType RestaurantType { get; set; } = FoodType.Both;
        
        [Required]
        public RestaurantCategory RestaurantCategory { get; set; } = RestaurantCategory.Restaurant;
    }
}
