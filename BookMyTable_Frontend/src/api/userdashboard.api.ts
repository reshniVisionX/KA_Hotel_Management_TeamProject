import { http } from '../Service/https';
import type { ApiSuccessResponse } from '../Types/Dashboard/ApiSuccessResponse';
import type { UserDeliveryAddress, CreateDeliveryAddressRequest } from '../Types/UserDashboard/UserDeliveryAddress';
import type { Order } from '../Types/UserDashboard/Orders';
import type { WishlistItem, CreateWishlistRequest } from '../Types/UserDashboard/Wishlist';
import type { TableBooking } from '../Types/UserDashboard/TableBooking';

// Delivery Address APIs
export const deliveryAddressApi = {
  getUserAddresses: async (userId: number): Promise<ApiSuccessResponse<any[]>> => {
    const response = await http.get(`/UserAddress/user/${userId}`);
    return response.data;
  },

  addAddress: async (data: CreateDeliveryAddressRequest): Promise<ApiSuccessResponse<UserDeliveryAddress>> => {
    const response = await http.post('/UserAddress', data);
    return response.data;
  },

  removeAddress: async (addressId: number): Promise<ApiSuccessResponse<boolean>> => {
    const response = await http.delete(`/DeliveryAddress/${addressId}`);
    return response.data;
  }
};

// Orders APIs
export const ordersApi = {
  getUserOrders: async (userId: number): Promise<ApiSuccessResponse<Order[]>> => {
    const response = await http.get(`/OrderHistory/user/${userId}`);
    return response.data;
  }
};

// Wishlist APIs
export const wishlistApi = {
  getUserWishlist: async (userId: number): Promise<ApiSuccessResponse<any[]>> => {
    const response = await http.get(`/Wishlists/user/${userId}`);
    return response.data;
  },

  addToWishlist: async (data: CreateWishlistRequest): Promise<ApiSuccessResponse<WishlistItem>> => {
    const response = await http.post('/Wishlists', data);
    return response.data;
  },

  removeFromWishlist: async (wishlistId: number): Promise<ApiSuccessResponse<boolean>> => {
    const response = await http.delete(`/Wishlists/${wishlistId}`);
    return response.data;
  },

  removeWishlistItem: async (userId: number, itemId: number): Promise<ApiSuccessResponse<boolean>> => {
    const response = await http.delete(`/Wishlists/user/${userId}/item/${itemId}`);
    return response.data;
  }
};

// Table Booking APIs
export const tableBookingApi = {
  getUserTableBookings: async (userId: number): Promise<ApiSuccessResponse<TableBooking[]>> => {
    const response = await http.get(`/TableBookings/user/${userId}`);
    return response.data;
  }
};

// Reviews APIs
export const reviewsApi = {
  getUserReviews: async (userId: number): Promise<ApiSuccessResponse<any[]>> => {
    const response = await http.get(`/Reviews/user/${userId}`);
    return response.data;
  }
};