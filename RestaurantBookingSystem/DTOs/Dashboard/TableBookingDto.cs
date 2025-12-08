using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTO
{
    public class TableBookingDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationInTime { get; set; }
        public TimeSpan ReservationOutTime { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ReseveredAt { get; set; }
        public bool AdvancePayment { get; set; }
        public string? UserName { get; set; }
        public string? TableNo { get; set; }
        public int? RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
    }
}