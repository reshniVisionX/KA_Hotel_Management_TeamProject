using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOorder
{
    public class TableReservationRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan ReservationInTime { get; set; }    

        [Required]
        public TimeSpan ReservationOutTime { get; set; }   

        [Required]
        [Range(1, 20)]
        public int NumberOfGuests { get; set; }

        public string? SpecialRequests { get; set; }

        public string BookingType { get; set; } = "TimeSlot"; 
    }

    public class CustomTimeReservationRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public TimeSpan ArrivalTime { get; set; }         

        [Required]
        [Range(1, 8)]
        public int DurationHours { get; set; }            

        [Required]
        [Range(1, 20)]
        public int NumberOfGuests { get; set; }

        public string? SpecialRequests { get; set; }
    }

    public class TimeSlotDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DisplayText { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class TableReservationResponseDto
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public string TableNo { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationInTime { get; set; }
        public TimeSpan ReservationOutTime { get; set; }
        public string Status { get; set; }
        public string RestaurantName { get; set; }
        public decimal AdvanceAmount { get; set; }
        public bool IsAdvancePaid { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class AdvancePaymentRequestDto
    {
        [Required]
        public int ReservationId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }
    }

    public class AdvancePaymentResponseDto
    {
        public int PaymentId { get; set; }
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }

    public class TableAvailabilityDto
    {
        public int TableId { get; set; }
        public string TableNo { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
    }
}