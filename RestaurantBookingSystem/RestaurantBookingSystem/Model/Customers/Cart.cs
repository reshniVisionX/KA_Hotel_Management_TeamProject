using RestaurantBookingSystem.Model.Restaurant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBookingSystem.Model.Customers
{
    // hold the item added to the cart from diff restaurant to order at once.
    // cartId must be cleared once it is ordered and pushed to the orders table
    public class Cart
    {
     [Key]
    public int CartId { get; set; }

    [Required]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [ForeignKey(nameof(MenuList))]
    public int ItemId { get; set; }

    [Required]
    [ForeignKey(nameof(Restaurant))]
    public int RestaurantId { get; set; }

    [Required]
    [Range(1, 50)]
    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Users? User { get; set; }
    public MenuList? MenuList { get; set; }        // Assuming MenuList is your menu item class
    public Restaurants? Restaurant { get; set; }
}

}
