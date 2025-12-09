namespace RestaurantBookingSystem.Dtos
{
    public class CustomerSummaryDto
    {
        public int TotalCustomers { get; set; }
        public int RecentCustomers { get; set; }
        public int FrequentCustomers { get; set; }
        public int NewCustomersThisMonth { get; set; }
    }
}