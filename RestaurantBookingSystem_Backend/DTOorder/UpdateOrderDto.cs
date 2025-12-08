using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOorder
{
    public class UpdateOrderDto
    {
        public List<OrderItemDto>? Items { get; set; }
        public OrderType OrderType { get; set; }
        public int QtyOrdered { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
