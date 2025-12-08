namespace RestaurantBookingSystem.Dtos
{
    public class UpdateMenuListsDto
    {
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public int? AvailableQty { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public bool? IsVegetarian { get; set; }
        public decimal? Tax { get; set; }
        public byte[]? Image { get; set; }
    }
}