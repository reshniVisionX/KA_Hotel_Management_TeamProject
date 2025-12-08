namespace RestaurantBookingSystem.Dtos
{
    public class ReviewsDto
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public decimal Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}