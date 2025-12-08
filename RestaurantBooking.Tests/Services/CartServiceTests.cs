using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.DTOorder;

using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Utils;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBooking.Tests.Services
{
    public class CartServiceTests
    {
        private readonly Mock<ICartRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CartService _service;

        public CartServiceTests()
        {
            _mockRepo = new Mock<ICartRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new CartService(_mockRepo.Object, _mockMapper.Object);
        }

      
        // ADD TO CART — NEW ITEM
        
        [Fact]
        public async Task AddToCartAsync_ShouldAddNewItem()
        {
            var request = new CartRequestDto { ItemId = 1, RestaurantId = 10 };

            _mockRepo.Setup(r => r.GetUserCartAsync(5)).ReturnsAsync(new List<Cart>());
            _mockRepo.Setup(r => r.GetCartItemAsync(5, 1)).ReturnsAsync((Cart)null);

            var savedCart = new Cart { CartId = 1, ItemId = 1, RestaurantId = 10, Quantity = 1 };
            _mockRepo.Setup(r => r.AddToCartAsync(It.IsAny<Cart>())).ReturnsAsync(savedCart);

            _mockMapper.Setup(m => m.Map<CartResponseDto>(savedCart))
                .Returns(new CartResponseDto { CartId = 1 });

            var result = await _service.AddToCartAsync(5, request);

            Assert.Equal(1, result.CartId);
        }

      
        // ADD TO CART — DIFFERENT RESTAURANT ERROR
     
        [Fact]
        public async Task AddToCartAsync_ShouldThrow_WhenDifferentRestaurant()
        {
            var existingItems = new List<Cart>
            {
                new Cart { RestaurantId = 20 }
            };

            _mockRepo.Setup(r => r.GetUserCartAsync(5)).ReturnsAsync(existingItems);

            var request = new CartRequestDto { ItemId = 1, RestaurantId = 10 };

            await Assert.ThrowsAsync<AppException>(() => _service.AddToCartAsync(5, request));
        }

        // ADD TO CART — INCREMENT EXISTING ITEM
      
        [Fact]
        public async Task AddToCartAsync_ShouldIncrementExistingItem()
        {
            var existingItem = new Cart { CartId = 1, ItemId = 1, RestaurantId = 10, Quantity = 1 };

            _mockRepo.Setup(r => r.GetUserCartAsync(5)).ReturnsAsync(new List<Cart> { existingItem });
            _mockRepo.Setup(r => r.GetCartItemAsync(5, 1)).ReturnsAsync(existingItem);

            _mockRepo.Setup(r => r.UpdateCartItemAsync(existingItem))
                .ReturnsAsync(existingItem);

            _mockMapper.Setup(m => m.Map<CartResponseDto>(existingItem))
                .Returns(new CartResponseDto { CartId = 1, Quantity = 2 });

            var result = await _service.AddToCartAsync(5, new CartRequestDto { ItemId = 1, RestaurantId = 10 });

            Assert.Equal(2, result.Quantity);
        }

      
        // UPDATE CART ITEM
      
        [Fact]
        public async Task UpdateCartItemAsync_ShouldReturnNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetCartItemAsync(5, 1)).ReturnsAsync((Cart)null);

            var result = await _service.UpdateCartItemAsync(5, 1, new CartUpdateDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCartItemAsync_ShouldUpdateItem()
        {
            var existing = new Cart { CartId = 1, Quantity = 1 };
            var updated = new Cart { CartId = 1, Quantity = 3 };

            _mockRepo.Setup(r => r.GetCartItemAsync(5, 1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateCartItemAsync(existing)).ReturnsAsync(updated);

            _mockMapper.Setup(m => m.Map<CartResponseDto>(updated))
                .Returns(new CartResponseDto { CartId = 1, Quantity = 3 });

            var result = await _service.UpdateCartItemAsync(5, 1, new CartUpdateDto { Quantity = 3 });

            Assert.Equal(3, result.Quantity);
        }

    
       
        // REMOVE FROM CART
      
        [Fact]
        public async Task RemoveFromCartAsync_ShouldReturnTrue()
        {
            _mockRepo.Setup(r => r.RemoveFromCartAsync(1)).ReturnsAsync(true);

            var result = await _service.RemoveFromCartAsync(1);

            Assert.True(result);
        }

        
        // GET USER CART
        
        [Fact]
        public async Task GetUserCartAsync_ShouldReturnCorrectSummary()
        {
            var cartItems = new List<Cart>
    {
        new Cart { CartId = 1, UserId = 1, ItemId = 10, RestaurantId = 5, Quantity = 2, AddedAt = DateTime.Now }
    };

            var menuItem = new MenuList
            {
                ItemId = 10,
                ItemName = "Pizza",
                Price = 50,      // IMPORTANT
                IsVegetarian = false
            };

            var restaurant = new Restaurants
            {
                RestaurantId = 5,
                RestaurantName = "Test Restaurant"
            };

            _mockRepo.Setup(r => r.GetUserCartAsync(1))
                     .ReturnsAsync(cartItems);

            _mockRepo.Setup(r => r.GetMenuItemAsync(10))
                     .ReturnsAsync(menuItem);  // ✔ Fix: Return an item with price

            _mockRepo.Setup(r => r.GetRestaurantAsync(5))
                     .ReturnsAsync(restaurant);

            var result = await _service.GetUserCartAsync(1);

            Assert.Equal(100, result.GrandTotal);  // 50 * 2
            Assert.Equal(2, result.TotalItems);
            Assert.Single(result.Items);
        }

     
        // INCREMENT QUANTITY
      
        [Fact]
        public async Task IncrementQuantityAsync_ShouldThrow_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetCartItemByIdAsync(1)).ReturnsAsync((Cart)null);

            await Assert.ThrowsAsync<AppException>(() => _service.IncrementQuantityAsync(1));
        }

        [Fact]
        public async Task IncrementQuantityAsync_ShouldIncrement()
        {
            var existing = new Cart { CartId = 1, Quantity = 1 };

            _mockRepo.Setup(r => r.GetCartItemByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateCartItemAsync(existing)).ReturnsAsync(existing);

            _mockMapper.Setup(m => m.Map<CartResponseDto>(existing))
                .Returns(new CartResponseDto { Quantity = 2 });

            var result = await _service.IncrementQuantityAsync(1);

            Assert.Equal(2, result.Quantity);
        }

       
        // DECREMENT QUANTITY
       
        [Fact]
        public async Task DecrementQuantityAsync_ShouldThrow_WhenBelowOne()
        {
            var existing = new Cart { CartId = 1, Quantity = 1 };

            _mockRepo.Setup(r => r.GetCartItemByIdAsync(1)).ReturnsAsync(existing);

            await Assert.ThrowsAsync<AppException>(() => _service.DecrementQuantityAsync(1));
        }

        [Fact]
        public async Task DecrementQuantityAsync_ShouldDecrement()
        {
            var existing = new Cart { CartId = 1, Quantity = 5 };
            _mockRepo.Setup(r => r.GetCartItemByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateCartItemAsync(existing)).ReturnsAsync(existing);

            _mockMapper.Setup(m => m.Map<CartResponseDto>(existing))
                .Returns(new CartResponseDto { Quantity = 4 });

            var result = await _service.DecrementQuantityAsync(1);

            Assert.Equal(4, result.Quantity);
        }
    }
}
