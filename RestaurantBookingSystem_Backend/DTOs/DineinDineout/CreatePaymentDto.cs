namespace RestaurantBookingSystem.DTO
{
    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PayMethod PayMethod { get; set; }
    }
}
