using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Delivery;

namespace RestaurantBookingSystem.Services
{
    public class DeliveryAddressService
    {
        private readonly IDeliveryAddress _repository;

        public DeliveryAddressService(IDeliveryAddress repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DeliveryAddress>> GetUserAddressesAsync(int userId)
        {
            return await _repository.GetUserAddresses(userId);
        }

        public async Task<DeliveryAddress?> GetAddressByIdAsync(int addressId)
        {
            return await _repository.GetAddressByIdAsync(addressId);
        }

        public async Task<DeliveryAddress?> GetDefaultAddressAsync(int userId)
        {
            return await _repository.GetDefaultAddressAsync(userId);
        }

        public async Task<DeliveryAddress> AddAddressAsync(DeliveryAddress address)
        {
            return await _repository.AddAddressAsync(address);
        }

        public async Task<DeliveryAddress> UpdateAddressAsync(DeliveryAddress address)
        {
            return await _repository.UpdateAddressAsync(address);
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            return await _repository.DeleteAddressAsync(addressId);
        }

        public async Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
            return await _repository.SetDefaultAddressAsync(userId, addressId);
        }
    }
}
