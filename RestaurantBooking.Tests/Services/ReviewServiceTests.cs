using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Utils;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBooking.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IReviewRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ReviewService _service;

        public ReviewServiceTests()
        {
            _mockRepo = new Mock<IReviewRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new ReviewService(_mockRepo.Object, _mockMapper.Object);
        }

        // -----------------------------------------------------
        // CREATE REVIEW
        // -----------------------------------------------------
        [Fact]
        public async Task CreateReviewAsync_ShouldReturnMappedResponse()
        {
            var dto = new ReviewCreateDto
            {
                RestaurantId = 10,
                Rating = 8,
                Comments = "Good food!"
            };

            var review = new Review
            {
                ReviewId = 1,
                UserId = 5,
                RestaurantId = 10,
                Rating = 8,
                Comments = "Good food!"
            };

            _mockRepo.Setup(r => r.CreateReviewAsync(It.IsAny<Review>()))
                .ReturnsAsync(review);

            _mockRepo.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync(review);

            _mockMapper.Setup(m => m.Map<ReviewResponseDto>(review))
                .Returns(new ReviewResponseDto { ReviewId = 1 });

            var result = await _service.CreateReviewAsync(5, dto);

            Assert.Equal(1, result.ReviewId);
        }

        // -----------------------------------------------------
        // GET RESTAURANT REVIEWS
        // -----------------------------------------------------
        [Fact]
        public async Task GetRestaurantReviewsAsync_ShouldReturnMappedList()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Rating = 7 }
            };

            _mockRepo.Setup(r => r.GetRestaurantReviewsAsync(10))
                .ReturnsAsync(reviews);

            _mockMapper.Setup(m => m.Map<List<ReviewResponseDto>>(reviews))
                .Returns(new List<ReviewResponseDto> { new ReviewResponseDto { ReviewId = 1 } });

            var result = await _service.GetRestaurantReviewsAsync(10);

            Assert.Single(result);
        }

        // -----------------------------------------------------
        // GET USER REVIEWS
        // -----------------------------------------------------
        [Fact]
        public async Task GetUserReviewsAsync_ShouldReturnMappedList()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 2, Rating = 9 }
            };

            _mockRepo.Setup(r => r.GetUserReviewsAsync(5))
                .ReturnsAsync(reviews);

            _mockMapper.Setup(m => m.Map<List<ReviewResponseDto>>(reviews))
                .Returns(new List<ReviewResponseDto> { new ReviewResponseDto { ReviewId = 2 } });

            var result = await _service.GetUserReviewsAsync(5);

            Assert.Single(result);
        }

        // -----------------------------------------------------
        // UPDATE REVIEW
        // -----------------------------------------------------
        [Fact]
        public async Task UpdateReviewAsync_ShouldReturnNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync((Review)null);

            var result = await _service.UpdateReviewAsync(1, new ReviewUpdateDto());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateReviewAsync_ShouldReturnUpdatedReview()
        {
            var existing = new Review { ReviewId = 1, Rating = 5 };
            var updated = new Review { ReviewId = 1, Rating = 9 };

            _mockRepo.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync(existing);

            _mockRepo.Setup(r => r.UpdateReviewAsync(existing))
                .ReturnsAsync(updated);

            _mockMapper.Setup(m => m.Map<ReviewResponseDto>(updated))
                .Returns(new ReviewResponseDto { ReviewId = 1, Rating = 9 });

            var result = await _service.UpdateReviewAsync(1, new ReviewUpdateDto { Rating = 9 });

            Assert.Equal(9, result.Rating);
        }

        // -----------------------------------------------------
        // DELETE REVIEW
        // -----------------------------------------------------
        [Fact]
        public async Task DeleteReviewAsync_ShouldReturnTrue()
        {
            _mockRepo.Setup(r => r.DeleteReviewAsync(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteReviewAsync(1);

            Assert.True(result);
        }

        // -----------------------------------------------------
        // AVERAGE RATING
        // -----------------------------------------------------
        [Fact]
        public async Task GetAverageRatingAsync_ShouldReturnCorrectValues()
        {
            var reviews = new List<Review>
{
    new Review { Rating = 9, Restaurant = new Restaurants { RestaurantName = "Test" } },
    new Review { Rating = 7, Restaurant = new Restaurants { RestaurantName = "Test" } },
    new Review { Rating = 5, Restaurant = new Restaurants { RestaurantName = "Test" } }
};

            _mockRepo.Setup(r => r.GetRestaurantReviewsAsync(10))
                     .ReturnsAsync(reviews);

            var result = await _service.GetAverageRatingAsync(10);

            Assert.Equal(10, result.RestaurantId);
            Assert.Equal(3, result.TotalReviews);
            Assert.Equal("Test", result.RestaurantName);

        }

        // -----------------------------------------------------
        // ITEM REVIEW CREATION
        // -----------------------------------------------------
        [Fact]
        public async Task CreateItemReviewAsync_ShouldThrow_WhenItemNotValid()
        {
            _mockRepo.Setup(r => r.ValidateItemExistsAsync(1, 10))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<AppException>(() =>
                _service.CreateItemReviewAsync(5, new ItemReviewCreateDto
                {
                    ItemId = 1,
                    RestaurantId = 10,
                    Rating = 8
                })
            );
        }

        [Fact]
        public async Task CreateItemReviewAsync_ShouldReturnMappedReview()
        {
            var dto = new ItemReviewCreateDto
            {
                ItemId = 1,
                RestaurantId = 10,
                Rating = 9,
                Comments = "Nice"
            };

            var created = new Review { ReviewId = 1 };

            _mockRepo.Setup(r => r.ValidateItemExistsAsync(1, 10))
                .ReturnsAsync(true);

            _mockRepo.Setup(r => r.CreateReviewAsync(It.IsAny<Review>()))
                .ReturnsAsync(created);

            _mockRepo.Setup(r => r.GetReviewByIdAsync(1))
                .ReturnsAsync(created);

            _mockMapper.Setup(m => m.Map<ReviewResponseDto>(created))
                .Returns(new ReviewResponseDto { ReviewId = 1 });

            var result = await _service.CreateItemReviewAsync(5, dto);

            Assert.Equal(1, result.ReviewId);
        }
    }
}
