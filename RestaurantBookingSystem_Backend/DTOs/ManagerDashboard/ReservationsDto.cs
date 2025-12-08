using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class ReservationsDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public int TableId { get; set; }
        public string? TableNo { get; set; }
        public int TableCapacity { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationInTime { get; set; }
        public TimeSpan ReservationOutTime { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ReseveredAt { get; set; }
        public bool AdvancePayment { get; set; }
    }
}