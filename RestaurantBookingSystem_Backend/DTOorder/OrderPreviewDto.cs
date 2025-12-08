namespace RestaurantBookingSystem.DTOorder
{
    public class OrderPreviewDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<OrderPreviewItemDto> Items { get; set; } = new();
        public int TotalQuantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderPreviewItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public int Quantity { get; set; }
        public decimal ItemSubtotal { get; set; }
        public decimal ItemTax { get; set; }
        public decimal ItemTotal { get; set; }
    }

    public class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderSummaryItemDto> Items { get; set; } = new();
        public int TotalQuantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class OrderSummaryItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal TaxPercentage { get; set; }
        public int Quantity { get; set; }
        public decimal ItemSubtotal { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal ItemAfterDiscount { get; set; }
        public decimal ItemTax { get; set; }
        public decimal ItemTotal { get; set; }
    }
}