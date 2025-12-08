using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantBookingSystem.Model.Customers
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }

        [Phone]
        public string? Mobile { get; set; }

        [Required]
        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; } = 1;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        // Navigation property
        public Roles? Role { get; set; }
    }
}