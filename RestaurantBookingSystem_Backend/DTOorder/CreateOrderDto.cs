using RestaurantBookingSystem.Model.Restaurant;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOorder
{
    public class CreateOrderDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public OrderType OrderType { get; set; }   // DineIn / DineOut
    }
}
