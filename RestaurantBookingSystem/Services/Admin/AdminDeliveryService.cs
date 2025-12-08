using RestaurantBookingSystem.Interfaces.Admin;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Utils;

namespace RestaurantBookingSystem.Services.Admin
{
    public class AdminDeliveryService: IAdminDeliveryService
    {
        private readonly IDeliveryRespository _deliveryRepo;

        public AdminDeliveryService(IDeliveryRespository repo)
        {
            _deliveryRepo = repo;
        }
        public async Task<IEnumerable<DeliveryAddress>> GetUserAddresses(int userId)
        {
            var addresses = await _deliveryRepo.GetUserAddressesAsync(userId);
            return addresses;
        }

        public async Task<DeliveryAddress> AddAddress(DeliveryAddress address)
        {
            var existingAddresses = await _deliveryRepo.GetUserAddressesAsync(address.UserId);

            foreach (var addr in existingAddresses)
            {
                addr.IsDefault = false;
            }

            address.IsDefault = true;

            await _deliveryRepo.AddAsync(address);
            await _deliveryRepo.SaveChangesAsync();

            return address;
        }

        public async Task<bool> ChangeDefaultAddress(int userId, int deliveryAddressId)
        {
            var addresses = await _deliveryRepo.GetUserAddressesAsync(userId);

            if (!addresses.Any())
                throw new AppException("User has no delivery addresses.");

            var targetAddress = addresses.FirstOrDefault(a => a.AddressId == deliveryAddressId);

            if (targetAddress == null)
                throw new AppException("Delivery address not found for this user.");

            foreach (var a in addresses)
                a.IsDefault = false;

            targetAddress.IsDefault = true;

            await _deliveryRepo.SaveChangesAsync();

            return true;
        }
    }
}
