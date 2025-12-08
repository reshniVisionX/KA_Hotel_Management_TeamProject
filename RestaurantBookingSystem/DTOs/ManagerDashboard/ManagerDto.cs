namespace RestaurantBookingSystem.Dtos
{
    public class ManagerDto
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}