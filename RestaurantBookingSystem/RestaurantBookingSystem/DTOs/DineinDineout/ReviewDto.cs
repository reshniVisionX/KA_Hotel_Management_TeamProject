using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs
{
    public class ReviewCreateDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public string? Comments { get; set; }
    }

    public class ReviewUpdateDto
    {
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

    public class ReviewRatingDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
    }

    public class ItemReviewCreateDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(0, 10)]
        public decimal Rating { get; set; }

        public string? Comments { get; set; }
    }

    public class ItemRatingDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}