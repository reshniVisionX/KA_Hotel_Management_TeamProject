using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOorder
{
    public class WishlistRequestDto
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
    }

    public class WishlistResponseDto
    {
        public int WishlistId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string ItemName { get; set; }
        public string RestaurantName { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}