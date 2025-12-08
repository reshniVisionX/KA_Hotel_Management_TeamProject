using Microsoft.EntityFrameworkCore;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interfaces.IRepository;
using RestaurantBookingSystem.Model.Manager;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Repository
{
    public class AdminManagerRepository : IManagerRequestRepository
    {
        private readonly BookingContext _context;
        private readonly EmailToManager _emailService;
        private readonly ILogger<AdminManagerRepository> _logger;

        public AdminManagerRepository(BookingContext context, EmailToManager email, ILogger<AdminManagerRepository> logger)
        {
            _context = context;
            _emailService = email;
            _logger = logger;
        }

        // ------------------- PAYOUTS -------------------
        public async Task<bool> ProcessMonthlyPayoutToManagersAsync(ManagerPayment payment)
        {          

            await _context.ManagerPayments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ManagerPayment>> GetPayoutHistoryAsync(int managerId)
        {
            return await _context.ManagerPayments
                .Where(p => p.ManagerId == managerId)
              
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        // ------------------- MANAGER VERIFICATION -------------------
        // get all unverified manager along with the restaurant details
        public async Task<IEnumerable<ManagerDetails>> GetAllUnverifiedManagersAsync()
        {
            return await _context.ManagerDetails
                .Include(m => m.Restaurants)
                .Where(m => m.Verification == IsVerified.Unverified)
                .ToListAsync();
        }

        public async Task<bool> VerifyManagerAsync(int managerId, bool isVerified)
        {
            var manager = await _context.ManagerDetails.FindAsync(managerId);
            if (manager == null)
                return false;

            try
            {
                // ✅ 1️⃣ Update verification status
                manager.Verification = isVerified ? IsVerified.Verified : IsVerified.Rejected;
                manager.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                // ✅ 2️⃣ Prepare email details
                string subject = isVerified
                    ? "🎉 Manager Verification Approved"
                    : "❌ Manager Verification Rejected";

                string message = isVerified
                    ? $"Hello {manager.ManagerName},\n\nYour manager account has been successfully verified.\nYou can now access all platform features.\n\nRegards,\nRestaurant Booking System Team"
                    : $"Hello {manager.ManagerName},\n\nUnfortunately, your verification request has been rejected.\nPlease contact support for further assistance.\n\nRegards,\nRestaurant Booking System Team";

                // ✅ 3️⃣ Try sending email
                try
                {
                    var emailSent = await _emailService.SendEmailAsync(manager.Email, subject, message);
                    if (emailSent)
                    {
                        _logger.LogInformation("📧 Verification email sent successfully to {Email}", manager.Email);
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Email sending failed for ManagerId: {ManagerId}, Email: {Email}", managerId, manager.Email);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error verifying ManagerId: {ManagerId}", managerId);
                    
                    Console.WriteLine($"[Email Error] Failed to send verification email to {manager.Email}: {ex.Message}");
                }

                return true;
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"[DB Error] Failed to verify manager ID {managerId}: {ex.Message}");
                return false;
            }
        }


        public async Task<IEnumerable<ManagerDetails>> FilterManagersAsync(bool isActive, IsVerified? verification)
        {
            _logger.LogInformation("Filtering managers by Active: {Active}, Verification: {Verification}", isActive, verification);

            var query = _context.ManagerDetails.AsQueryable();

            query = query.Where(m => m.IsActive == isActive);

            if (verification.HasValue)
                query = query.Where(m => m.Verification == verification.Value);

            return await query.ToListAsync();
        }
    }
}
