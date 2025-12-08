namespace RestaurantBookingSystem.DTOorder
{
    public class CreateMenuListDto
    {
        public int RestaurantId { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
    }
}
