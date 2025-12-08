import {http} from "./https";
import { handleAxiosError } from "../Service/handleApiError";
import type { Restaurant } from "../Types/Restaurant/Restaurants";
import type { ApiSuccessResponse } from "../Types/Dashboard/ApiSuccessResponse";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { DeliveryAddress } from "../Types/Restaurant/DeliveryAddress";
import type { CreateDeliveryAddress } from "../Types/Restaurant/CreateDeliveryAddress";
import type { ChangeDeliveryAddress } from "../Types/Restaurant/ChangeDeliveryAddress";
import type { AppError } from "../Types/Dashboard/ApiError";
import type { MenuItem } from "../Types/Restaurant/MenuItems";
import type { CartRequest } from "../Types/Restaurant/CartRequest";
import type { CartResponse, CartItem, OrderResponse, OrderSummary, PaymentRequest, PaymentResponse } from "../Types/Restaurant/CartItems";
import type { DeliveryPerson } from "../Types/Restaurant/DeliveryPerson";
import type { DeliveryPersonHistory, CompleteDeliveryResponse } from "../Types/Restaurant/DeliveryPersonHistory";
import type { OrderHistory } from "../Types/Restaurant/OrderHistory";
import type { WishlistItem, AddToWishlistRequest } from "../Types/Restaurant/Wishlist";
import type { AddReviewRequest } from "../Types/Restaurant/AddReview";
import type { ManagerRegisterRequest } from "../Types/Restaurant/ManageRestaurant";

const API_URL = import.meta.env.VITE_API_URL || "";

function validateResponse<T>(
  response: ApiSuccessResponse<T> | ApiErrorResponse
): ApiSuccessResponse<T> {

  if (response.success === true) {
    return response;
  }
  console.log("Error from API:", response.message);
  throw {
 
    message: response.message,
    status: 400
  } as AppError;
}

export async function getDeliveryAddresses(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<DeliveryAddress[]> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/forUser/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function createDeliveryAddress(payload: CreateDeliveryAddress) {
  try {
    const res = await http.post<
      ApiSuccessResponse<DeliveryAddress> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/createAddress`, payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function changeDefaultAddress(payload: ChangeDeliveryAddress) {
  try {
    const res = await http.post<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/changeDefault`, payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getAllRestaurants() {
  try {
    const res = await http.get<
      ApiSuccessResponse<Restaurant[]> | ApiErrorResponse
    >(`${API_URL}/Restaurant`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getAllMenuItems() {
  try {
    const res = await http.get<
      ApiSuccessResponse<MenuItem[]> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/get-all-menu`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function addToCart(userId: number, cartRequest: CartRequest) {
  try {
    const res = await http.post<
      ApiSuccessResponse<object> | ApiErrorResponse
    >(`${API_URL}/Cart/add/${userId}`, cartRequest);
   console.log(res.data);
    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function getCartItems(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<CartResponse> | ApiErrorResponse
    >(`${API_URL}/Cart/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function removeFromCart(cartId: number) {
  try {
    const res = await http.delete<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Cart/remove/${cartId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function clearCart(userId: number) {
  try {
    const res = await http.delete<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Cart/clear/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function incrementCartItem(cartId: number) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<CartItem> | ApiErrorResponse
    >(`${API_URL}/Cart/increment/${cartId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function decrementCartItem(cartId: number) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<CartItem> | ApiErrorResponse
    >(`${API_URL}/Cart/decrement/${cartId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function placeOrder(userId: number) {
  try {
    const res = await http.post<
      ApiSuccessResponse<OrderResponse> | ApiErrorResponse
    >(`${API_URL}/Orders/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderSummary(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderSummary> | ApiErrorResponse
    >(`${API_URL}/Orders/summary/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function processPayment(paymentData: PaymentRequest) {
  try {
    const res = await http.post<
      ApiSuccessResponse<PaymentResponse> | ApiErrorResponse
    >(`${API_URL}/Payment`, paymentData);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function getAllDeliveryPersons() {
  try {
    const res = await http.get<
      ApiSuccessResponse<DeliveryPerson[]> | ApiErrorResponse
    >(`${API_URL}/DeliveryPerson`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function completeDelivery(deliveryId: number) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<CompleteDeliveryResponse> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/complete-delivery/${deliveryId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getDeliveryPersonHistory(deliveryPersonId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<DeliveryPersonHistory[]> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/person/${deliveryPersonId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function getOrderHistory(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderHistory[]> | ApiErrorResponse
    >(`${API_URL}/OrderHistory/user/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function addToWishlist(userId: number, wishlistRequest: AddToWishlistRequest) {
  try {
    const res = await http.post<
      ApiSuccessResponse<WishlistItem> | ApiErrorResponse
    >(`${API_URL}/Wishlist/add/${userId}`, wishlistRequest);
    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getUserWishlist(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<WishlistItem[]> | ApiErrorResponse
    >(`${API_URL}/Wishlist/user/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function removeFromWishlist(wishlistId: number) {
  try {
    const res = await http.delete<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Wishlist/remove/${wishlistId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function addReview(reviewData: AddReviewRequest) {
  try {
    const res = await http.post<
      ApiSuccessResponse<boolean> | ApiErrorResponse
    >(`${API_URL}/Reviews`, reviewData);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function registerManagerWithRestaurant(data: ManagerRegisterRequest) {
  try {
    const formData = new FormData();
    
    // Manager fields
    formData.append('ManagerName', data.managerName);
    formData.append('UserId', data.userId.toString());
    formData.append('Email', data.email);
    formData.append('PhoneNumber', data.phoneNumber);
    formData.append('Password', data.password);
    
    // Restaurant fields
    formData.append('RestaurantName', data.restaurantName);
    if (data.description) formData.append('Description', data.description);
    if (data.ratings) formData.append('Ratings', data.ratings.toString());
    formData.append('RestaurantCategory', data.restaurantCategory.toString());
    formData.append('RestaurantType', data.restaurantType.toString());
    formData.append('Location', data.location);
    formData.append('City', data.city);
    formData.append('ContactNo', data.contactNo);
    if (data.deliveryCharge) formData.append('DeliveryCharge', data.deliveryCharge.toString());
    formData.append('IsActive', data.isActive.toString());
    
    // File
    if (data.image) {
      formData.append('Image', data.image);
    }

    const res = await http.post<
      ApiSuccessResponse<object> | ApiErrorResponse
    >(`${API_URL}/ManagerRegistration/RegisterManagerWithRestaurant`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}
