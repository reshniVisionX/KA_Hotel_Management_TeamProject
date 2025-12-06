using System.ComponentModel.DataAnnotations;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOs
{
    public class ReservationCreateDto
    {
        [Required]
        public int TableId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan ReservationInTime { get; set; }

        [Required]
        public TimeSpan ReservationOutTime { get; set; }

        public decimal AdvancePaymentAmount { get; set; } = 0;

        public bool AdvancePayment { get; set; } = false;
    }

    public class ReservationUpdateDto
    {
        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan ReservationInTime { get; set; }

        [Required]
        public TimeSpan ReservationOutTime { get; set; }

        public decimal AdvancePaymentAmount { get; set; }

        public bool AdvancePayment { get; set; }
    }

    public class ReservationResponseDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int TableId { get; set; }
        public string TableNo { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationInTime { get; set; }
        public TimeSpan ReservationOutTime { get; set; }
        public ReservationStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; }
        public decimal AdvancePaymentAmount { get; set; }
        public bool AdvancePayment { get; set; }
    }


}