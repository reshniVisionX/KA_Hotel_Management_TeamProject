using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Dtos
{
    public class UpdateDineInDto
    {
        public string? TableNo { get; set; }
        public int? Capacity { get; set; }
        public TableStatus? Status { get; set; }
    }
}