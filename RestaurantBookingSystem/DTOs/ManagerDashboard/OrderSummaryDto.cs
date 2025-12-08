namespace RestaurantBookingSystem.Dtos
{
    public class OrderSummaryDto
    {
        public int TotalOrders { get; set; }
        public int TodayOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
    }
}