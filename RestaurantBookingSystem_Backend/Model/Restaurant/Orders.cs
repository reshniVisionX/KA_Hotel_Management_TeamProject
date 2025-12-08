using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Model.Restaurant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json; // For JSON serialization

namespace RestaurantBookingSystem.Model.Restaurant
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(Restaurant))]
        public int RestaurantId { get; set; }

        public Restaurants? Restaurant { get; set; }

        [Required]
        public string ItemsList { get; set; } = string.Empty;

        [NotMapped]
        public List<ItemQuantity>? Items
        {
            get => string.IsNullOrEmpty(ItemsList)
                ? new List<ItemQuantity>()
                : JsonSerializer.Deserialize<List<ItemQuantity>>(ItemsList);
            set => ItemsList = JsonSerializer.Serialize(value);
        }

        [Required]
        public OrderType OrderType { get; set; } = OrderType.DineIn;

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public Users? User { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QtyOrdered { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 99999999.99)]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;
    }

    public class ItemQuantity
    {
        [Required]
        public int ItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }

    public enum OrderStatus
    {
        Pending,//0
        InProgress,//1
        Completed,//2
        Cancelled//3
    }

    public enum OrderType
    {
        DineIn,
        DineOut,
        TakeAway
    }
}