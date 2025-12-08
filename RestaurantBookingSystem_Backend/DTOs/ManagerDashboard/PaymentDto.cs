using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PayMethod PayMethod { get; set; }
        public PaymentStatus Status { get; set; }

    }
}