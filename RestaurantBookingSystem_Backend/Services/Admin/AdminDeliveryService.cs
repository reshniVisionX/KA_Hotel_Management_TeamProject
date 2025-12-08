using RestaurantBookingSystem.DTOs.Admin;
using RestaurantBookingSystem.Interfaces.Admin;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Model.Restaurant;
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
        public async Task<IEnumerable<MenuList>> GetAllMenuListAsync()
        {
            return await _deliveryRepo.GetAllMenuListAsync();
        }

        public async Task<bool> CompleteDeliveryAsync(int deliveryId)
        {
            var delivery = await _deliveryRepo.GetDeliveryWithRelationsAsync(deliveryId)
                           ?? throw new AppException("Delivery not found");

            if (delivery.Order == null)
                throw new AppException("Order not found for this delivery");

            if (delivery.DeliveryPerson == null)
                throw new AppException("Delivery person not found");

            // 1. Update delivery
            delivery.DeliveryStatus = DeliveryStatus.Delivered;
            delivery.DeliveredAt = DateTime.Now;

            await _deliveryRepo.UpdateDeliveryAsync(delivery);

            // 2. Update delivery person
            delivery.DeliveryPerson.Status = DeliveryPersonStatus.Available;
            await _deliveryRepo.UpdateDeliveryPersonAsync(delivery.DeliveryPerson);

            // 3. Update order
            delivery.Order.Status = OrderStatus.Completed;
            await _deliveryRepo.UpdateOrderAsync(delivery.Order);

            return true;
        }
        public async Task<List<DeliveryPersonHistory>> GetDeliveriesForPersonAsync(int deliveryPersonId)
        {
            var deliveries = await _deliveryRepo.GetDeliveriesByPersonAsync(deliveryPersonId);

            if (deliveries == null)
                throw new AppException("You havent done any deliveries");

            var result = deliveries.Select(d => new DeliveryPersonHistory 
            {
                DeliveryId = d.DeliveryId,
                DeliveryStatus = d.DeliveryStatus,
                EstimatedDeliveryTime = d.EstimatedDeliveryTime,

                // Delivery Address
                Mobile = d.DeliveryAddress?.Mobile,
                Address = d.DeliveryAddress?.Address,
                City = d.DeliveryAddress?.City,
                State = d.DeliveryAddress?.State,
                Pincode = d.DeliveryAddress?.Pincode,
                Landmark = d.DeliveryAddress?.Landmark,

                // Customer Name
                CustomerFirstName = d.DeliveryAddress?.User?.FirstName,
                CustomerLastName = d.DeliveryAddress?.User?.LastName,

                // Delivery Person
                DeliveryPersonId = d.DeliveryPersonId,
                DeliveryName = d.DeliveryPerson?.DeliveryName ?? "",
                LicenseNumber = d.DeliveryPerson?.LicenseNumber ?? "",
                Otp = d.DeliveryPerson?.otp,
                Status = d.DeliveryPerson?.Status ?? DeliveryPersonStatus.Available,
                TotalDeliveries = d.DeliveryPerson?.TotalDeliveries ?? 0,
                AverageRating = d.DeliveryPerson?.AverageRating ?? 0
            }).ToList();

            return result;
        }

    }
}
