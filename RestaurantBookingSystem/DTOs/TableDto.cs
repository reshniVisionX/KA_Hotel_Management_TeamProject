using System.ComponentModel.DataAnnotations;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.DTOs
{


    public class TableResponseDto
    {
        public int TableId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string TableNo { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public TableStatus Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
    }


}