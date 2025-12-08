using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOorder
{
    public class DeliveryRatingDto
    {
        [Required]
        public int DeliveryId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public double Rating { get; set; }
    }

    public class DeliveryCompletionDto
    {
        [Required]
        public int DeliveryId { get; set; }

        [Required]
        public int Otp { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public double? Rating { get; set; }
    }
}