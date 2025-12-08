using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _userAddressRepo;

        public UserAddressService(IUserAddressRepository userAddressRepo)
        {
            _userAddressRepo = userAddressRepo;
        }

        public async Task<List<UserAddressDto>> GetAddressesByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new AppException("User ID must be greater than 0");

            var addresses = await _userAddressRepo.GetAddressesByUserIdAsync(userId);
            return addresses.Select(a => new UserAddressDto
            {
                AddressId = a.AddressId,
                UserId = a.UserId,
                Mobile = a.Mobile,
                Address = a.Address,
                City = a.City,
                State = a.State,
                Pincode = a.Pincode,
                Landmark = a.Landmark,
                ContactNo = a.ContactNo,
                IsDefault = a.IsDefault
            }).ToList();
        }
    }
}