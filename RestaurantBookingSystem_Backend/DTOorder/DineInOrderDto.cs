using System.ComponentModel.DataAnnotations;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOorder
{
    public class DineInOrderRequestDto
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int ReservationId { get; set; }

        [Required]
        public List<ItemQuantity> Items { get; set; } = new List<ItemQuantity>();

        [Required]
        public int QtyOrdered { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }

    public class DineInOrderResponseDto
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public string TableNo { get; set; }
        public List<ItemQuantity> Items { get; set; }
        public int UserId { get; set; }
        public int QtyOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
    }
}