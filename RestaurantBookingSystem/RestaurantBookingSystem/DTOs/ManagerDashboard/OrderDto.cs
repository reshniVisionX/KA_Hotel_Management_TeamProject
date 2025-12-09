using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public List<ItemQuantity>? Items { get; set; }
        public OrderType OrderType { get; set; }
        public int UserId { get; set; }
        public int QtyOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}