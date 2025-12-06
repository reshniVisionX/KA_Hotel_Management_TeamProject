using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _repository;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WishlistResponseDto> AddToWishlistAsync(int userId, WishlistCreateDto request)
        {
            // Check if item already in wishlist
            var existingWishlistItem = await _repository.CheckItemInWishlistAsync(userId, request.ItemId);
            if (existingWishlistItem != null)
            {
                throw new InvalidOperationException("Item is already in your wishlist");
            }

            var wishlist = new Wishlist
            {
                UserId = userId,
                ItemId = request.ItemId,
                RestaurantId = request.RestaurantId,
                CreatedAt = DateTime.Now
            };

            var savedWishlist = await _repository.AddToWishlistAsync(wishlist);
            return await _repository.MapToResponseDtoAsync(savedWishlist);
        }

        public async Task<bool> RemoveFromWishlistAsync(int wishlistId)
        {
            return await _repository.RemoveFromWishlistAsync(wishlistId);
        }

        public async Task<List<WishlistResponseDto>> GetUserWishlistAsync(int userId)
        {
            return await _repository.GetUserWishlistWithDetailsAsync(userId);
        }

        public async Task<WishlistCheckDto> CheckItemInWishlistAsync(int userId, int itemId)
        {
            var wishlistItem = await _repository.CheckItemInWishlistAsync(userId, itemId);
            
            return new WishlistCheckDto
            {
                IsInWishlist = wishlistItem != null,
                WishlistId = wishlistItem?.WishlistId
            };
        }


    }
}