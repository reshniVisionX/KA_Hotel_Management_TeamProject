namespace RestaurantBookingSystem.DTOorder
{
    public class CartRequestDto
    {
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
      
    }

    public class CartUpdateDto
    {
        public int Quantity { get; set; }
    }

    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string ItemName { get; set; }
        public string RestaurantName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsVegetarian { get; set; }
        public DateTime AddedAt { get; set; }
    }

    public class CartSummaryDto
    {
        public List<CartResponseDto> Items { get; set; } = new();
        public decimal GrandTotal { get; set; }
        public int TotalItems { get; set; }
    }

    public class CartGroupDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public List<CartResponseDto> Items { get; set; } = new();
        public decimal RestaurantTotal { get; set; }
        public int RestaurantItemCount { get; set; }
    }
}