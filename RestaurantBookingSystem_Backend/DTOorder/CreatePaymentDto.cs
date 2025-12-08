namespace RestaurantBookingSystem.DTOorder
{
    public class CreatePaymentDto
    {
        public int OrderId { get; set; }
   
        public PayMethod PayMethod { get; set; }
    }
}
