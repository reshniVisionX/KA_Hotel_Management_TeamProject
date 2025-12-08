using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Model.Delivery;
using RestaurantBookingSystem.Utils;

namespace RestaurantBooking.Tests.Services
{
    public class DeliveryServiceTests
    {
        private readonly Mock<IDelivery> _mockRepo;
        private readonly DeliveryService _service;

        public DeliveryServiceTests()
        {
            _mockRepo = new Mock<IDelivery>();
            _service = new DeliveryService(_mockRepo.Object);
        }

        // ---------------------------------------------------------
        // CREATE DELIVERY
        // ---------------------------------------------------------
        [Fact]
        public async Task CreateDeliveryAsync_ShouldThrow_WhenOrderIdInvalid()
        {
            var dto = new DeliveryCreateDto { OrderId = 0, AddressId = 1 };

            await Assert.ThrowsAsync<AppException>(() =>
                _service.CreateDeliveryAsync(dto));
        }

        [Fact]
        public async Task CreateDeliveryAsync_ShouldThrow_WhenAddressIdInvalid()
        {
            var dto = new DeliveryCreateDto { OrderId = 1, AddressId = 0 };

            await Assert.ThrowsAsync<AppException>(() =>
                _service.CreateDeliveryAsync(dto));
        }

        [Fact]
        public async Task CreateDeliveryAsync_ShouldReturnMappedResponse()
        {
            var dto = new DeliveryCreateDto { OrderId = 1, AddressId = 1 };

            var delivery = new Delivery
            {
                DeliveryId = 10,
                OrderId = 1,
                DeliveryPerson = new DeliveryPerson { DeliveryName = "John", MobileNo = "99999" },
                DeliveryAddress = new DeliveryAddress { Address = "Street 1", City = "Hyd", Pincode = "500001" },
                DeliveryStatus = DeliveryStatus.OutForDelivery
            };

            _mockRepo.Setup(r => r.CreateDeliveryAsync(1, 1))
                .ReturnsAsync(delivery);

            var result = await _service.CreateDeliveryAsync(dto);

            Assert.Equal(10, result.DeliveryId);
            Assert.Equal("John", result.DeliveryPersonName);
        }

        // ---------------------------------------------------------
        // GET DELIVERY BY ID
        // ---------------------------------------------------------
        [Fact]
        public async Task GetDeliveryByIdAsync_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetDeliveryByIdAsync(1))
                .ReturnsAsync((Delivery)null);

            await Assert.ThrowsAsync<AppException>(() =>
                _service.GetDeliveryByIdAsync(1));
        }

        [Fact]
        public async Task GetDeliveryByIdAsync_ShouldReturnMappedDto()
        {
            var delivery = new Delivery
            {
                DeliveryId = 1,
                OrderId = 2,
                DeliveryAddress = new DeliveryAddress { Address = "Test", City = "Hyd", Pincode = "500001" }
            };

            _mockRepo.Setup(r => r.GetDeliveryByIdAsync(1))
                .ReturnsAsync(delivery);

            var result = await _service.GetDeliveryByIdAsync(1);

            Assert.Equal(1, result.DeliveryId);
            Assert.Equal("Hyd", result.City);
        }

        // ---------------------------------------------------------
        // GET DELIVERIES BY USER
        // ---------------------------------------------------------
        [Fact]
        public async Task GetDeliveriesByUserAsync_ShouldReturnList()
        {
            var deliveries = new List<Delivery>
            {
                new Delivery { DeliveryId = 1, OrderId = 2 },
                new Delivery { DeliveryId = 2, OrderId = 3 }
            };

            _mockRepo.Setup(r => r.GetDeliveriesByUserIdAsync(5))
                .ReturnsAsync(deliveries);

            var result = await _service.GetDeliveriesByUserAsync(5);

            Assert.Collection(result,
                item => Assert.Equal(1, item.DeliveryId),
                item => Assert.Equal(2, item.DeliveryId)
            );
        }

        // ---------------------------------------------------------
        // UPDATE DELIVERY STATUS
        // ---------------------------------------------------------
        [Fact]
        public async Task UpdateDeliveryStatusAsync_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.UpdateDeliveryStatusAsync(10, DeliveryStatus.Delivered))
                .ReturnsAsync((Delivery)null);

            await Assert.ThrowsAsync<AppException>(() =>
                _service.UpdateDeliveryStatusAsync(10, new DeliveryStatusUpdateDto { Status = DeliveryStatus.Delivered }));
        }

        [Fact]
        public async Task UpdateDeliveryStatusAsync_ShouldReturnUpdatedResult()
        {
            var d = new Delivery { DeliveryId = 10, DeliveryStatus = DeliveryStatus.OutForDelivery };

            _mockRepo.Setup(r => r.UpdateDeliveryStatusAsync(10, DeliveryStatus.Delivered))
                .ReturnsAsync(d);

            var result = await _service.UpdateDeliveryStatusAsync(10,
                new DeliveryStatusUpdateDto { Status = DeliveryStatus.Delivered });

            Assert.Equal(10, result.DeliveryId);
        }

        // ---------------------------------------------------------
        // COMPLETE DELIVERY (OTP VALIDATION)
        // ---------------------------------------------------------
        [Fact]
        public async Task CompleteDeliveryAsync_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetDeliveryByIdAsync(10))
                .ReturnsAsync((Delivery)null);

            await Assert.ThrowsAsync<AppException>(() =>
                _service.CompleteDeliveryAsync(new DeliveryCompletionDto { DeliveryId = 10, Otp = 1234 }));
        }

        [Fact]
        public async Task CompleteDeliveryAsync_ShouldThrow_WhenOtpInvalid()
        {
            var delivery = new Delivery
            {
                DeliveryId = 10,
                DeliveryPerson = new DeliveryPerson { otp = 9999 }
            };

            _mockRepo.Setup(r => r.GetDeliveryByIdAsync(10))
                .ReturnsAsync(delivery);

            await Assert.ThrowsAsync<AppException>(() =>
                _service.CompleteDeliveryAsync(
                    new DeliveryCompletionDto { DeliveryId = 10, Otp = 1234 }
                ));
        }

        [Fact]
        public async Task CompleteDeliveryAsync_ShouldReturnSuccessMessage()
        {
            var delivery = new Delivery
            {
                DeliveryId = 10,
                DeliveryPerson = new DeliveryPerson { otp = 1234 }
            };

            _mockRepo.Setup(r => r.GetDeliveryByIdAsync(10))
                .ReturnsAsync(delivery);

            _mockRepo.Setup(r => r.UpdateDeliveryStatusAsync(10, DeliveryStatus.Delivered))
                .ReturnsAsync(delivery);

            var result = await _service.CompleteDeliveryAsync(
                new DeliveryCompletionDto { DeliveryId = 10, Otp = 1234 });

            Assert.Equal("Delivery completed successfully", result);
        }
    }
}
