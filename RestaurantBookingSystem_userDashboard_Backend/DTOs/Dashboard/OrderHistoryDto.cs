using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTO
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public string ItemsList { get; set; } = string.Empty;
        public OrderType OrderType { get; set; }
        public int QtyOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus? Status { get; set; }
        public string? UserName { get; set; }
        public string? RestaurantName { get; set; }
    }
}