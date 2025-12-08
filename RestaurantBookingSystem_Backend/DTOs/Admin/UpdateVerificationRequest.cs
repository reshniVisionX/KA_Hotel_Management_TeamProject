namespace RestaurantBookingSystem.DTOs.Admin
{
    public class UpdateVerificationRequest
    {
        public int ManagerId { get; set; }
        public IsVerified VerificationStatus { get; set; }
    }
}
