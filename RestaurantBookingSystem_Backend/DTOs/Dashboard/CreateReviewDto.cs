using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class CreateReviewDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Comments must be between 10 and 1000 characters")]
        public string Comments { get; set; } = string.Empty;
    }
}