using RestaurantBookingSystem.Model.Manager;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBookingSystem.Model.Restaurant
{
    public class Restaurants
    {
        [Key]
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(150)]
        public string RestaurantName { get; set; } = string.Empty;

        public byte[]? Images { get; set; }

        public string? Description { get; set; }

        [Range(0, 10)]
        public decimal? Ratings { get; set; }

        [Required]
        public RestaurantCategory RestaurantCategory { get; set; } = RestaurantCategory.Restaurant;

        [Required]
        public FoodType RestaurantType { get; set; } = FoodType.Both;

        [Required]
        [StringLength(505)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Phone]
        public string ContactNo { get; set; } = string.Empty;

        [Range(0, 9999)]
        public decimal? DeliveryCharge { get; set; }

        [Required]
        [ForeignKey(nameof(Manager))]
        public int ManagerId { get; set; }

        public ManagerDetails? Manager { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public ICollection<MenuList>? MenuLists { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

public enum FoodType
{
    Veg,
    Nonveg,
    Both
}

public enum RestaurantCategory
{
    Cafe,
    FastFood,
    Restaurant,
    Desserts,
    Chat
}