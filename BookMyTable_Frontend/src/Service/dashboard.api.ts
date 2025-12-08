import type {  CreateDeliveryAddressRequest } from "../Types/UserDashboard/UserDeliveryAddress";
import type {  CreateWishlistRequest } from "../Types/UserDashboard/Wishlist";
import { deliveryAddressApi, ordersApi, wishlistApi, tableBookingApi, reviewsApi } from "../api/userdashboard.api";
import { store } from "../redux/store";

// Delivery Address APIs
export async function getUserAddresses(userId: number) {
  return await deliveryAddressApi.getUserAddresses(userId);
}

export async function addAddress(payload: CreateDeliveryAddressRequest) {
  return await deliveryAddressApi.addAddress(payload);
}

export async function removeAddress(addressId: number) {
  const userId = store.getState().auth.user?.userId;
  if (!userId) {
    throw new Error("User not authenticated");
  }
  
  const addressesResponse = await deliveryAddressApi.getUserAddresses(userId);
  const addresses = addressesResponse.data;
  
  const addressToDelete = addresses.find(addr => addr.addressId === addressId);
  
  if (addressToDelete?.isDefault) {
    throw new Error("Default address can't be deleted");
  }
  return await deliveryAddressApi.removeAddress(addressId);
}

// Orders APIs
export async function getUserOrders(userId: number) {
  return await ordersApi.getUserOrders(userId);
}

// Wishlist APIs
export async function getUserWishlist(userId: number) {
  return await wishlistApi.getUserWishlist(userId);
}

export async function addToWishlist(payload: CreateWishlistRequest) {
  return await wishlistApi.addToWishlist(payload);
}

export async function removeFromWishlist(wishlistId: number) {
  return await wishlistApi.removeFromWishlist(wishlistId);
}

export async function removeWishlistItem(userId: number, itemId: number) {
  return await wishlistApi.removeWishlistItem(userId, itemId);
}

// Table Booking APIs
export async function getUserTableBookings(userId: number) {
  return await tableBookingApi.getUserTableBookings(userId);
}

// Reviews APIs
export async function getUserReviews(userId: number) {
  return await reviewsApi.getUserReviews(userId);
}

