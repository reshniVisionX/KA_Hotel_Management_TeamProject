using RestaurantBookingSystem.Model.Restaurant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBookingSystem.Model.Customers
{
    // this holds the liked dish for the user
    public class Wishlist
    {
        [Key]
        public int WishlistId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Required]
        [ForeignKey(nameof(Item))]
        public int ItemId { get; set; }

        [Required]
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }

        public Users? User { get; set; }

        public MenuList? Item { get; set; }          // navigation for ItemId
        public Restaurants? Restaurant { get; set; } // navigation for RestaurantId

        public DateTime CreatedAt { get; set; }
    }
}