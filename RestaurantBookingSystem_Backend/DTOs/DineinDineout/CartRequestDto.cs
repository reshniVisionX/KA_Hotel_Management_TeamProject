using System.ComponentModel.DataAnnotations;

namespace RestaurantBookingSystem.DTOs
{
    public class CartRequestDto
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

       
    }

    public class CartUpdateDto
    {
        [Required]
        [Range(1, 50)]
        public int Quantity { get; set; }
    }

    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }
        public int RestaurantId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
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
        public int TotalQuantity { get; set; }
    }



    public class CartTotalDto
    {
        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class CheckCartItemDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
    }

    public class CartItemExistsDto
    {
        public bool IsInCart { get; set; }
        public int Quantity { get; set; }
    }
}