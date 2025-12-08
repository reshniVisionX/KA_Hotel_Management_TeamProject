using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOorder
{
    public class ReviewRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public string? Comments { get; set; }
    }

    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public decimal Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? UserName { get; set; }
        public string? RestaurantName { get; set; }
    }
}