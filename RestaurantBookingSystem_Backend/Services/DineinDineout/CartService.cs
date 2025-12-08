using AutoMapper;
using RestaurantBookingSystem.DTOorder;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Customers;
using RestaurantBookingSystem.Utils;

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
            // Check if user already has items in cart from different restaurant
            var existingCartItems = await _repository.GetUserCartAsync(userId);
            if (existingCartItems.Any())
            {
                var existingRestaurantId = existingCartItems.First().RestaurantId;
                if (existingRestaurantId != request.RestaurantId)
                {
                    throw new AppException("Cannot add item from a different restaurant. Please choose items from the same restaurant.");
                }
            }

            var existingItem = await _repository.GetCartItemAsync(userId, request.ItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
                existingItem.AddedAt = DateTime.Now;
                await _repository.UpdateCartItemAsync(existingItem);
                return await MapToResponseDto(existingItem);
            }

            var cart = new Cart
            {
                UserId = userId,
                ItemId = request.ItemId,
                RestaurantId = request.RestaurantId,
                Quantity = 1,
                AddedAt = DateTime.Now
            };

            var savedCart = await _repository.AddToCartAsync(cart);
            return await MapToResponseDto(savedCart);
        }

        public async Task<CartResponseDto> UpdateCartItemAsync(int userId, int itemId, CartUpdateDto request)
        {
            var cartItem = await _repository.GetCartItemAsync(userId, itemId);
            if (cartItem == null) return null;

            cartItem.Quantity = request.Quantity;
            cartItem.AddedAt = DateTime.Now;

            var updatedCart = await _repository.UpdateCartItemAsync(cartItem);
            return await MapToResponseDto(updatedCart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartId)
        {
            return await _repository.RemoveFromCartAsync(cartId);
        }

        public async Task<CartSummaryDto> GetUserCartAsync(int userId)
        {
            var cartItems = await _repository.GetUserCartAsync(userId);
            var responseItems = new List<CartResponseDto>();
            decimal grandTotal = 0;
            int totalItems = 0;

            foreach (var item in cartItems)
            {
                var responseDto = await MapToResponseDto(item);
                responseItems.Add(responseDto);
                grandTotal += responseDto.TotalPrice;
                totalItems += item.Quantity;
            }

            return new CartSummaryDto
            {
                Items = responseItems,
                GrandTotal = grandTotal,
                TotalItems = totalItems
            };
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            return await _repository.ClearCartAsync(userId);
        }

        public async Task<CartResponseDto> IncrementQuantityAsync(int cartId)
        {
            var cartItem = await _repository.GetCartItemByIdAsync(cartId);
            if (cartItem == null)
                throw new AppException($"Cart item with ID {cartId} not found");

            // Validate max quantity (optional business rule)
            if (cartItem.Quantity >= 50)
                throw new AppException("Maximum quantity limit reached (50)");

            cartItem.Quantity += 1;
            cartItem.AddedAt = DateTime.Now;

            var updatedCart = await _repository.UpdateCartItemAsync(cartItem);
            return await MapToResponseDto(updatedCart);
        }

        public async Task<CartResponseDto> DecrementQuantityAsync(int cartId)
        {
            var cartItem = await _repository.GetCartItemByIdAsync(cartId);
            if (cartItem == null)
                throw new AppException($"Cart item with ID {cartId} not found");

            // Validate minimum quantity
            if (cartItem.Quantity <= 1)
                throw new AppException("Cannot decrement below 1. Use remove instead");

            cartItem.Quantity -= 1;
            cartItem.AddedAt = DateTime.Now;

            var updatedCart = await _repository.UpdateCartItemAsync(cartItem);
            return await MapToResponseDto(updatedCart);
        }

        public async Task<CartSummaryDto> SearchCartAsync(int userId, string? restaurantName, bool? isVegetarian, decimal? minPrice, decimal? maxPrice)
        {
            var cartItems = await _repository.GetUserCartAsync(userId);
            var filteredItems = new List<CartResponseDto>();
            decimal grandTotal = 0;
            int totalItems = 0;

            foreach (var item in cartItems)
            {
                var menuItem = await _repository.GetMenuItemAsync(item.ItemId);
                var restaurant = await _repository.GetRestaurantAsync(item.RestaurantId);

                if (menuItem == null || restaurant == null) continue;

                // Apply filters
                if (!string.IsNullOrEmpty(restaurantName) &&
                    !restaurant.RestaurantName.Contains(restaurantName, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (isVegetarian.HasValue && menuItem.IsVegetarian != isVegetarian.Value)
                    continue;

                if (minPrice.HasValue && menuItem.Price < minPrice.Value)
                    continue;

                if (maxPrice.HasValue && menuItem.Price > maxPrice.Value)
                    continue;

                var responseDto = await MapToResponseDto(item);
                filteredItems.Add(responseDto);
                grandTotal += responseDto.TotalPrice;
                totalItems += item.Quantity;
            }

            return new CartSummaryDto
            {
                Items = filteredItems,
                GrandTotal = grandTotal,
                TotalItems = totalItems
            };
        }

        public async Task<List<CartGroupDto>> GetCartGroupedByRestaurantAsync(int userId)
        {
            var cartItems = await _repository.GetUserCartAsync(userId);
            var groupedResult = new List<CartGroupDto>();
            var restaurantGroups = cartItems.GroupBy(x => x.RestaurantId);

            foreach (var group in restaurantGroups)
            {
                var restaurant = await _repository.GetRestaurantAsync(group.Key);
                if (restaurant == null) continue;

                var groupItems = new List<CartResponseDto>();
                decimal restaurantTotal = 0;
                int restaurantItemCount = 0;

                foreach (var item in group)
                {
                    var responseDto = await MapToResponseDto(item);
                    groupItems.Add(responseDto);
                    restaurantTotal += responseDto.TotalPrice;
                    restaurantItemCount += item.Quantity;
                }

                groupedResult.Add(new CartGroupDto
                {
                    RestaurantId = group.Key,
                    RestaurantName = restaurant.RestaurantName,
                    Items = groupItems,
                    RestaurantTotal = restaurantTotal,
                    RestaurantItemCount = restaurantItemCount
                });
            }

            return groupedResult.OrderBy(x => x.RestaurantName).ToList();
        }

        private async Task<CartResponseDto> MapToResponseDto(Cart cart)
        {
            var menuItem = await _repository.GetMenuItemAsync(cart.ItemId);
            var restaurant = await _repository.GetRestaurantAsync(cart.RestaurantId);

            return new CartResponseDto
            {
                CartId = cart.CartId,
                ItemId = cart.ItemId,
                RestaurantId = cart.RestaurantId,
                ItemName = menuItem?.ItemName ?? "",
                RestaurantName = restaurant?.RestaurantName ?? "",
                Price = menuItem?.Price ?? 0,
                Quantity = cart.Quantity,
                TotalPrice = (menuItem?.Price ?? 0) * cart.Quantity,
                IsVegetarian = menuItem?.IsVegetarian ?? false,
                AddedAt = cart.AddedAt
            };
        }

    }
}