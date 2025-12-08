using RestaurantBookingSystem.Model.Delivery;
using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTO
{
    public class DeliveryStatusUpdateDto
    {
       

        [Required]
        public DeliveryStatus Status { get; set; }
    }
}
