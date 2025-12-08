namespace RestaurantBookingSystem.DTO
{
    public class WishlistDto
    {
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
    }
}