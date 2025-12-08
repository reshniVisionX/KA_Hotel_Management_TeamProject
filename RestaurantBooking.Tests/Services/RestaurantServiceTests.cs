using Moq;
using Xunit;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace RestaurantBooking.Tests.Services
{
    public class RestaurantServiceTests
    {
        private readonly Mock<IRestaurant> _mockRepo;
        private readonly RestaurantService _service;

        public RestaurantServiceTests()
        {
            _mockRepo = new Mock<IRestaurant>();
            _service = new RestaurantService(_mockRepo.Object);
        }

        // --------------------------
        // TEST: Get All Restaurants
        // --------------------------
        [Fact]
        public async Task GetAllRestaurantsAsync_ShouldReturnList()
        {
            _mockRepo.Setup(r => r.GetAllRestaurants())
                .ReturnsAsync(new List<Restaurants>
                {
                    new Restaurants { RestaurantId = 1, RestaurantName = "Test Resto" }
                });

            var result = await _service.GetAllRestaurantsAsync();

            Assert.Single(result);
        }

        // --------------------------
        // TEST: Get Restaurant By ID
        // --------------------------
        [Fact]
        public async Task GetRestaurantByIdAsync_ShouldReturnRestaurant()
        {
            _mockRepo.Setup(r => r.GetRestaurantById(1))
                .ReturnsAsync(new Restaurants { RestaurantId = 1 });

            var result = await _service.GetRestaurantByIdAsync(1);

            Assert.Equal(1, result.RestaurantId);
        }

        // --------------------------
        // TEST: Get Menu - Restaurant Does Not Exist
        // --------------------------
        [Fact]
        public async Task GetMenuByRestaurantIdAsync_ShouldThrow_WhenRestaurantNotFound()
        {
            _mockRepo.Setup(r => r.RestaurantExistsAsync(99)).ReturnsAsync(false);

            var ex = await Assert.ThrowsAsync<AppException>(() =>
                _service.GetMenuByRestaurantIdAsync(99)
            );

            Assert.Equal("Restaurant with ID 99 not found", ex.Message);
        }

        // --------------------------
        // TEST: Get Menu - No Menu Found
        // --------------------------
        [Fact]
        public async Task GetMenuByRestaurantIdAsync_ShouldThrow_WhenMenuIsEmpty()
        {
            _mockRepo.Setup(r => r.RestaurantExistsAsync(1)).ReturnsAsync(true);
            _mockRepo.Setup(r => r.GetMenuByRestaurantId(1)).ReturnsAsync(new List<MenuList>());

            var ex = await Assert.ThrowsAsync<AppException>(() =>
                _service.GetMenuByRestaurantIdAsync(1)
            );

            Assert.Equal("No menu items found for restaurant 1", ex.Message);
        }

        // --------------------------
        // TEST: Get Menu - Success
        // --------------------------
        [Fact]
        public async Task GetMenuByRestaurantIdAsync_ShouldReturnMenu()
        {
            _mockRepo.Setup(r => r.RestaurantExistsAsync(1)).ReturnsAsync(true);
            _mockRepo.Setup(r => r.GetMenuByRestaurantId(1))
                .ReturnsAsync(new List<MenuList>
                {
                    new MenuList { ItemId = 10, ItemName = "Pizza" }
                });

            var result = await _service.GetMenuByRestaurantIdAsync(1);

            Assert.Single(result);
        }

        // --------------------------
        // TEST: Search - No Results
        // --------------------------
        [Fact]
        public async Task SearchRestaurantsAsync_ShouldThrow_WhenNoResults()
        {
            _mockRepo.Setup(r => r.SearchRestaurantsAsync("KFC", "Delhi"))
                .ReturnsAsync(new List<Restaurants>());

            var ex = await Assert.ThrowsAsync<AppException>(() =>
                _service.SearchRestaurantsAsync("KFC", "Delhi")
            );

            Assert.Equal("No restaurants found for name 'KFC' and city 'Delhi'", ex.Message);
        }

        // --------------------------
        // TEST: Search - Success
        // --------------------------
        [Fact]
        public async Task SearchRestaurantsAsync_ShouldReturnRestaurants()
        {
            _mockRepo.Setup(r => r.SearchRestaurantsAsync("Dominos", "Hyderabad"))
                .ReturnsAsync(new List<Restaurants>
                {
                    new Restaurants { RestaurantId = 1, RestaurantName = "Dominos" }
                });

            var result = await _service.SearchRestaurantsAsync("Dominos", "Hyderabad");

            Assert.Single(result);
            Assert.Equal("Dominos", result.First().RestaurantName);
        }
    }
}
