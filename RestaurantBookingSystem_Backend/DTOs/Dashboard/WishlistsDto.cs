namespace RestaurantBookingSystem.DTO
{
    public class WishlistsDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }

        // Restaurant Details
        public string? RestaurantName { get; set; }
        public byte[]? RestaurantImage { get; set; }
        public string? RestaurantCategory { get; set; }
        public decimal? RestaurantRating { get; set; }
        public string? Location { get; set; }

        // Menu Item Details
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? ItemPrice { get; set; }
        public byte[]? ItemImage { get; set; }
        public bool? IsVegetarian { get; set; }
    }
}