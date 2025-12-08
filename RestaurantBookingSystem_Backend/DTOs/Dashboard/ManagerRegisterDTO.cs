public class ManagerRegisterDTO
{
    // Manager fields
    public string ManagerName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Restaurant fields
    public string RestaurantName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Ratings { get; set; }
    public RestaurantCategory RestaurantCategory { get; set; }
    public FoodType RestaurantType { get; set; }
    public string Location { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ContactNo { get; set; } = string.Empty;
    public decimal? DeliveryCharge { get; set; }
    public bool IsActive { get; set; } = true;

    // File
    public IFormFile? Image { get; set; }
}
