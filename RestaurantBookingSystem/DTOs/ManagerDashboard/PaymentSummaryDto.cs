namespace RestaurantBookingSystem.Dtos
{
    public class PaymentSummaryDto
    {
        public int TotalPayments { get; set; }
        public int TodayPayments { get; set; }
        public int CompletedPayments { get; set; }
        public int PendingPayments { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TodayAmount { get; set; }
        public int CashPayments { get; set; }
        public int UpiPayments { get; set; }
    }
}