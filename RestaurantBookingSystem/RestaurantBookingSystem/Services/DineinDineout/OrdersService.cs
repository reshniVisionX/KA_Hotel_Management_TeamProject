using RestaurantBookingSystem.DTO;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
using static RestaurantBookingSystem.Interface.IOrders;

namespace RestaurantBookingSystem.Services
{
    public class OrdersService
    {
        private readonly IOrders _repo;

        public OrdersService(IOrders repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Orders>> GetAllAsync()
            => await _repo.GetAllOrdersAsync();

        public async Task<Orders?> GetByIdAsync(int id)
            => await _repo.GetOrderByIdAsync(id);

        public async Task<OrderSummaryDto> GetOrderSummaryAsync(int userId)
        {
            // Get most recent pending order for user
            var pendingOrder = await _repo.GetUserPendingOrderAsync(userId);
            if (pendingOrder == null)
                throw new AppException($"No pending order found for user {userId}");

            var summaryItems = new List<OrderSummaryItemDto>();
            decimal subtotal = 0;
            decimal totalDiscount = 0;
            decimal totalTax = 0;
            int totalQuantity = 0;

            foreach (var orderItem in pendingOrder.Items)
            {
                var menuItem = await _repo.GetMenuItemAsync(orderItem.ItemId);
                
                var itemSubtotal = menuItem.Price * orderItem.Quantity;
                var itemDiscount = menuItem.Discount * orderItem.Quantity;
                var itemAfterDiscount = itemSubtotal - itemDiscount;
                var itemTax = itemAfterDiscount * (menuItem.Tax / 100);
                var itemTotal = itemAfterDiscount + itemTax;

                summaryItems.Add(new OrderSummaryItemDto
                {
                    ItemId = orderItem.ItemId,
                    ItemName = menuItem.ItemName,
                    OriginalPrice = menuItem.Price,
                    Discount = menuItem.Discount,
                    PriceAfterDiscount = menuItem.Price - menuItem.Discount,
                    TaxPercentage = menuItem.Tax,
                    Quantity = orderItem.Quantity,
                    ItemSubtotal = Math.Round(itemSubtotal, 2),
                    ItemDiscount = Math.Round(itemDiscount, 2),
                    ItemAfterDiscount = Math.Round(itemAfterDiscount, 2),
                    ItemTax = Math.Round(itemTax, 2),
                    ItemTotal = Math.Round(itemTotal, 2)
                });

                subtotal += itemSubtotal;
                totalDiscount += itemDiscount;
                totalTax += itemTax;
                totalQuantity += orderItem.Quantity;
            }

            var restaurant = await _repo.GetRestaurantAsync(pendingOrder.RestaurantId);

            return new OrderSummaryDto
            {
                OrderId = pendingOrder.OrderId,
                RestaurantId = pendingOrder.RestaurantId,
                RestaurantName = restaurant?.RestaurantName ?? "",
                OrderDate = pendingOrder.OrderDate,
                Items = summaryItems,
                TotalQuantity = totalQuantity,
                Subtotal = Math.Round(subtotal, 2),
                TotalDiscount = Math.Round(totalDiscount, 2),
                TotalAfterDiscount = Math.Round(subtotal - totalDiscount, 2),
                TotalTax = Math.Round(totalTax, 2),
                GrandTotal = Math.Round(subtotal - totalDiscount + totalTax, 2)
            };
        }

        public async Task<IEnumerable<Orders>> GetByRestaurantAsync(int restaurantId)
        {
            var restaurantExists = await _repo.RestaurantExistsAsync(restaurantId);
            if (!restaurantExists)
                throw new AppException($"Restaurant with ID {restaurantId} not found");

            var orders = await _repo.GetOrdersByRestaurantAsync(restaurantId);
            if (!orders.Any())
                throw new AppException($"No orders found for restaurant {restaurantId}");

            return orders;
        }

        public async Task<Orders> CreateAsync(CreateOrderDto dto)
        {
            // Basic validation
            if (dto.RestaurantId <= 0) throw new AppException("Invalid RestaurantId");
            if (dto.UserId <= 0) throw new AppException("Invalid UserId");

            // Check if restaurant exists
            var restaurantExists = await _repo.RestaurantExistsAsync(dto.RestaurantId);
            if (!restaurantExists)
                throw new AppException($"Restaurant with ID {dto.RestaurantId} not found");

            // Get cart items for this user and restaurant
            var cartItems = await _repo.GetUserCartItemsAsync(dto.UserId, dto.RestaurantId);
            if (!cartItems.Any())
                throw new AppException($"No items found in cart for user {dto.UserId} and restaurant {dto.RestaurantId}");

            // Calculate totals
            var orderItems = new List<ItemQuantity>();
            int totalQuantity = 0;
            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                // Get menu item details
                var menuItem = await _repo.GetMenuItemAsync(cartItem.ItemId);
                
                // Validate restaurant consistency
                if (menuItem.RestaurantId != dto.RestaurantId)
                    throw new AppException($"Item {cartItem.ItemId} does not belong to restaurant {dto.RestaurantId}");

                // Calculate item totals
                var itemPrice = menuItem.Price - menuItem.Discount;
                var itemSubtotal = itemPrice * cartItem.Quantity;
                var itemTax = itemSubtotal * (menuItem.Tax / 100);
                var itemTotal = itemSubtotal + itemTax;

                // Add to order
                orderItems.Add(new ItemQuantity { ItemId = cartItem.ItemId, Quantity = cartItem.Quantity });
                totalQuantity += cartItem.Quantity;
                totalAmount += itemTotal;
            }

            // Create order
            var order = new Orders
            {
                RestaurantId = dto.RestaurantId,
                UserId = dto.UserId,
                Items = orderItems,
                QtyOrdered = totalQuantity,
                TotalAmount = Math.Round(totalAmount, 2),
                OrderType = dto.OrderType,
                Status = OrderStatus.Pending
            };

            // Save order
            var createdOrder = await _repo.CreateOrderAsync(order);

            // Clear cart after successful order creation
            await _repo.ClearUserCartAsync(dto.UserId, dto.RestaurantId);

            return createdOrder;
        }

        public async Task<OrderPreviewDto> GetOrderPreviewAsync(int userId, int restaurantId)
        {
            // Check if restaurant exists
            var restaurantExists = await _repo.RestaurantExistsAsync(restaurantId);
            if (!restaurantExists)
                throw new AppException($"Restaurant with ID {restaurantId} not found");

            // Get cart items for this user and restaurant
            var cartItems = await _repo.GetUserCartItemsAsync(userId, restaurantId);
            if (!cartItems.Any())
                throw new AppException($"No items found in cart for user {userId} and restaurant {restaurantId}");

            var previewItems = new List<OrderPreviewItemDto>();
            int totalQuantity = 0;
            decimal subtotal = 0;
            decimal totalTax = 0;

            foreach (var cartItem in cartItems)
            {
                var menuItem = await _repo.GetMenuItemAsync(cartItem.ItemId);
                
                if (menuItem.RestaurantId != restaurantId)
                    throw new AppException($"Item {cartItem.ItemId} does not belong to restaurant {restaurantId}");

                var itemPrice = menuItem.Price - menuItem.Discount;
                var itemSubtotal = itemPrice * cartItem.Quantity;
                var itemTax = itemSubtotal * (menuItem.Tax / 100);
                var itemTotal = itemSubtotal + itemTax;

                previewItems.Add(new OrderPreviewItemDto
                {
                    ItemId = cartItem.ItemId,
                    ItemName = menuItem.ItemName,
                    Price = menuItem.Price,
                    Discount = menuItem.Discount,
                    Tax = menuItem.Tax,
                    Quantity = cartItem.Quantity,
                    ItemSubtotal = Math.Round(itemSubtotal, 2),
                    ItemTax = Math.Round(itemTax, 2),
                    ItemTotal = Math.Round(itemTotal, 2)
                });

                totalQuantity += cartItem.Quantity;
                subtotal += itemSubtotal;
                totalTax += itemTax;
            }

            return new OrderPreviewDto
            {
                RestaurantId = restaurantId,
                RestaurantName = "", // Can be populated if needed
                Items = previewItems,
                TotalQuantity = totalQuantity,
                Subtotal = Math.Round(subtotal, 2),
                TotalTax = Math.Round(totalTax, 2),
                TotalAmount = Math.Round(subtotal + totalTax, 2)
            };
        }

        public async Task<Orders> CreateFromCartAsync(int userId)
        {
            // Get all cart items for user
            var cartItems = await _repo.GetAllUserCartItemsAsync(userId);
            if (!cartItems.Any())
                throw new AppException($"No items found in cart for user {userId}");

            // Cancel all existing orders for this user
            await _repo.CancelUserOrdersAsync(userId);

            // Validate all items are from same restaurant
            var restaurantIds = new HashSet<int>();
            foreach (var cartItem in cartItems)
            {
                var menuItem = await _repo.GetMenuItemAsync(cartItem.ItemId);
                restaurantIds.Add(menuItem.RestaurantId);
            }

            if (restaurantIds.Count > 1)
                throw new AppException("All items in cart must be from the same restaurant");

            var restaurantId = restaurantIds.First();

            // Calculate totals
            var orderItems = new List<ItemQuantity>();
            int totalQuantity = 0;
            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                var menuItem = await _repo.GetMenuItemAsync(cartItem.ItemId);
                
                var itemPrice = menuItem.Price - menuItem.Discount;
                var itemSubtotal = itemPrice * cartItem.Quantity;
                var itemTax = itemSubtotal * (menuItem.Tax / 100);
                var itemTotal = itemSubtotal + itemTax;

                orderItems.Add(new ItemQuantity { ItemId = cartItem.ItemId, Quantity = cartItem.Quantity });
                totalQuantity += cartItem.Quantity;
                totalAmount += itemTotal;
            }

            // Create new order with Pending status
            var order = new Orders
            {
                RestaurantId = restaurantId,
                UserId = userId,
                Items = orderItems,
                QtyOrdered = totalQuantity,
                TotalAmount = Math.Round(totalAmount, 2),
                OrderType = OrderType.DineOut, // Default order type
                Status = OrderStatus.Pending
            };

            // Save order
            var createdOrder = await _repo.CreateOrderAsync(order);

            // Reduce inventory for ordered items
            foreach (var orderItem in orderItems)
            {
                await _repo.ReduceMenuItemQuantityAsync(orderItem.ItemId, orderItem.Quantity);
            }

            // Clear entire cart after successful order creation
            await _repo.ClearAllUserCartAsync(userId);

            return createdOrder;
        }
    }
}
