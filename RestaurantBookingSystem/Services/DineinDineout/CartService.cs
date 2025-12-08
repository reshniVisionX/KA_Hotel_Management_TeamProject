using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;

namespace RestaurantBookingSystem.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CartResponseDto> AddToCartAsync(int userId, CartRequestDto request)
        {
            var existingItem = await _repository.GetCartItemAsync(userId, request.ItemId, request.RestaurantId);
            
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.AddedAt = DateTime.Now;
                await _repository.UpdateCartItemAsync(existingItem);
                return await _repository.MapToResponseDtoAsync(existingItem);
            }

            var cart = new Cart
            {
                UserId = userId,
                ItemId = request.ItemId,
                RestaurantId = request.RestaurantId,
                Quantity = request.Quantity,
                AddedAt = DateTime.Now
            };

            var savedCart = await _repository.AddToCartAsync(cart);
            return await _repository.MapToResponseDtoAsync(savedCart);
        }

        public async Task<CartResponseDto?> UpdateCartItemAsync(int cartId, CartUpdateDto request)
        {
            var cartItem = await _repository.GetCartByIdAsync(cartId);
            if (cartItem == null) return null;

            cartItem.Quantity = request.Quantity;
            cartItem.AddedAt = DateTime.Now;
            
            var updatedCart = await _repository.UpdateCartItemAsync(cartItem);
            return await _repository.MapToResponseDtoAsync(updatedCart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            return await _repository.RemoveFromCartAsync(cartId);
        }

        public async Task<CartSummaryDto> GetUserCartAsync(int userId)
        {
            return await _repository.GetUserCartSummaryAsync(userId);
        }



        public async Task<bool> ClearCartAsync(int userId)
        {
            return await _repository.ClearCartAsync(userId);
        }

        public async Task<CartTotalDto> GetCartTotalAsync(int userId)
        {
            return await _repository.GetCartTotalAsync(userId);
        }

        public async Task<CartItemExistsDto> IsItemInCartAsync(CheckCartItemDto request)
        {
            var cartItem = await _repository.GetCartItemAsync(request.UserId, request.ItemId, request.RestaurantId);
            
            return new CartItemExistsDto
            {
                IsInCart = cartItem != null,
                Quantity = cartItem?.Quantity ?? 0
            };
        }


    }
}