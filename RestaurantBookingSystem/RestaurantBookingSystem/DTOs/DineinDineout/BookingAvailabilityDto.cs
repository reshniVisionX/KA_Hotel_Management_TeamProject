using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs
{
    public class BookingRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 20)]
        public int GuestCount { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }
    }

    public class TimeSlotAvailabilityDto
    {
        public string TimeSlot { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public int AvailableCapacity { get; set; }
        public int TotalCapacity { get; set; }
    }

    public class RestaurantAvailabilityDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public int RequestedGuests { get; set; }
        public List<TimeSlotAvailabilityDto> TimeSlots { get; set; } = new();
        public int TotalRestaurantCapacity { get; set; }
    }

    public class SmartBookingRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 20)]
        public int GuestCount { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        public decimal AdvancePaymentAmount { get; set; } = 0;
        public bool AdvancePayment { get; set; } = false;
    }

    public class RestaurantCapacityDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public int TotalTables { get; set; }
        public int TotalCapacity { get; set; }
        public List<TimeSlotCapacityDto> TimeSlotCapacities { get; set; } = new();
    }

    public class TimeSlotCapacityDto
    {
        public string TimeSlot { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalCapacity { get; set; }
        public int BookedCapacity { get; set; }
        public int AvailableCapacity { get; set; }
    }
}