import {http} from "./https";
import { handleAxiosError } from "./handleApiError";
import { tokenstore } from "../auth/tokenstore";
import type { ApiSuccessResponse } from "../Types/Dashboard/ApiSuccessResponse";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { Customer, CustomerSummary } from "../Types/Manager/Customer";
import type { Order, OrderSummary, DailyRevenue } from "../Types/Manager/Order";
import type { Payment, PaymentSummary } from "../Types/Manager/Payment";
import type { Restaurant, UpdateRestaurant, Manager } from "../Types/Manager/Restaurant";
import type { Reservation, ReservationSummary } from "../Types/Manager/Reservation";
import type { MenuItem, CreateMenuItem, UpdateMenuItem } from "../Types/Manager/Menu";
import type { AppError } from "../Types/Dashboard/ApiError";
import type { 
  DineInTable, 
  CreateDineInTable, 
  UpdateDineInTable, 
  OrderHistory, 
  Review, 
  RestaurantRating, 
  MenuItemRating, 
  PayoutData, 
  PayoutHistory, 
  RestaurantRevenue 
} from "../Types/Manager/DineIn";

const API_URL = import.meta.env.VITE_API_URL || "";

// Create axios instance with auth headers
const createAuthHeaders = () => {
  const token = tokenstore.getToken();
  return token ? { Authorization: `Bearer ${token}` } : {};
};

// Helper function to clear old tokens and force re-login
export const clearAuthData = () => {
  tokenstore.clear();
  // Also clear any old tokens that might exist
  document.cookie.split(";").forEach(function(c) { 
    document.cookie = c.replace(/^ +/, "").replace(/=.*/, "=;expires=" + new Date().toUTCString() + ";path=/"); 
  });
  localStorage.clear();
};

// Helper to validate backend response - handles both wrapped and direct responses
function validateResponse<T>(response: T | ApiSuccessResponse<T> | ApiErrorResponse): ApiSuccessResponse<T> {
  // If response has success property, it's wrapped
  if (typeof response === 'object' && response !== null && 'success' in response) {
    if (response.success === true) {
      return response;
    }
    throw {
      message: response.message,
      status: 400
    } as AppError;
  }
  
  // If response is direct data, wrap it
  return {
    success: true,
    data: response,
    message: 'Success'
  } as ApiSuccessResponse<T>;
}

// Customer endpoints
export async function getCustomersByRestaurant(restaurantId: number) {
  try {
    const res = await http.get<Customer[] | ApiSuccessResponse<Customer[]> | ApiErrorResponse>(
      `${API_URL}/Customer/restaurant/${restaurantId}`
    );

    return validateResponse<Customer[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getRecentCustomers(restaurantId: number, days: number = 30) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Customer[]> | ApiErrorResponse
    >(`${API_URL}/Customer/restaurant/${restaurantId}/recent?days=${days}`);

    return validateResponse<Customer[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getFrequentCustomers(restaurantId: number, minOrders: number = 3) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Customer[]> | ApiErrorResponse
    >(`${API_URL}/Customer/restaurant/${restaurantId}/frequent?minOrders=${minOrders}`);

    return validateResponse<Customer[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getCustomerSummary(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<CustomerSummary> | ApiErrorResponse
    >(`${API_URL}/Customer/restaurant/${restaurantId}/summary`);

    return validateResponse<CustomerSummary>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Order endpoints
export async function getOrdersByRestaurant(restaurantId: number, startDate?: string, endDate?: string, status?: string) {
  try {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    if (status) params.append('status', status);
    
    const res = await http.get<
      ApiSuccessResponse<Order[]> | ApiErrorResponse
    >(`${API_URL}/Order/restaurant/${restaurantId}?${params}`);

    return validateResponse<Order[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderById(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Order> | ApiErrorResponse
    >(`${API_URL}/Order/${id}`);

    return validateResponse<Order>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTodayOrders(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Order[]> | ApiErrorResponse
    >(`${API_URL}/Order/restaurant/${restaurantId}/today`);

    return validateResponse<Order[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderSummary(restaurantId: number, startDate?: string, endDate?: string) {
  try {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    
    const res = await http.get<
      ApiSuccessResponse<OrderSummary> | ApiErrorResponse
    >(`${API_URL}/Order/restaurant/${restaurantId}/summary?${params}`);

    return validateResponse<OrderSummary>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getDailyRevenue(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<DailyRevenue[]> | ApiErrorResponse
    >(`${API_URL}/Order/restaurant/${restaurantId}/daily-revenue`);

    return validateResponse<DailyRevenue[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Payment endpoints
export async function getPaymentsByRestaurant(restaurantId: number, startDate?: string, endDate?: string, paymentMethod?: string) {
  try {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    if (paymentMethod) params.append('paymentMethod', paymentMethod);
    
    const res = await http.get<
      ApiSuccessResponse<Payment[]> | ApiErrorResponse
    >(`${API_URL}/Payments/restaurant/${restaurantId}?${params}`);

    return validateResponse<Payment[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTodayPayments(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Payment[]> | ApiErrorResponse
    >(`${API_URL}/Payments/restaurant/${restaurantId}/today`);

    return validateResponse<Payment[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getPaymentSummary(restaurantId: number, startDate?: string, endDate?: string) {
  try {
    const params = new URLSearchParams();
    if (startDate) params.append('startDate', startDate);
    if (endDate) params.append('endDate', endDate);
    
    const res = await http.get<
      ApiSuccessResponse<PaymentSummary> | ApiErrorResponse
    >(`${API_URL}/Payments/restaurant/${restaurantId}/summary?${params}`);

    return validateResponse<PaymentSummary>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Reservation endpoints
export async function getReservationsByRestaurant(restaurantId: number, days: number = 30) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Reservation[]> | ApiErrorResponse
    >(`${API_URL}/Reservations/restaurant/${restaurantId}?days=${days}`);

    return validateResponse<Reservation[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTodayReservations(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Reservation[]> | ApiErrorResponse
    >(`${API_URL}/Reservations/today/${restaurantId}`);

    return validateResponse<Reservation[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getUpcomingReservations(restaurantId: number, days: number = 7) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Reservation[]> | ApiErrorResponse
    >(`${API_URL}/Reservations/upcoming/${restaurantId}?days=${days}`);

    return validateResponse<Reservation[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getReservationSummary(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<ReservationSummary> | ApiErrorResponse
    >(`${API_URL}/Reservations/summary/${restaurantId}`);

    return validateResponse<ReservationSummary>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getReservationById(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Reservation> | ApiErrorResponse
    >(`${API_URL}/Reservations/${id}`);

    return validateResponse<Reservation>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function updateReservationStatus(id: number, status: number) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<{ message: string }> | ApiErrorResponse
    >(`${API_URL}/Reservations/${id}/status`, { status });

    return validateResponse<{ message: string }>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Restaurant endpoints
export async function getAllRestaurants() {
  try {
    const res = await http.get<Restaurant[] | ApiSuccessResponse<Restaurant[]> | ApiErrorResponse>(
      `${API_URL}/Restaurants`
    );

    return validateResponse<Restaurant[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getRestaurantById(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Restaurant> | ApiErrorResponse
    >(`${API_URL}/Restaurants/${id}`);

    return validateResponse<Restaurant>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function updateRestaurant(id: number, data: UpdateRestaurant) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<Restaurant> | ApiErrorResponse
    >(`${API_URL}/Restaurants/${id}`, data);

    return validateResponse<Restaurant>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function updateRestaurantStatus(id: number, isActive: boolean) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<{ message: string }> | ApiErrorResponse
    >(`${API_URL}/Restaurants/${id}/status?isActive=${isActive}`);

    return validateResponse<{ message: string }>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getManagerRestaurant(userId: number) {
  try {
    // Get manager details by userId
    const managerRes = await http.get<
      ApiSuccessResponse<Manager> | ApiErrorResponse
    >(`${API_URL}/Restaurants/manager/user/${userId}`, {
      headers: createAuthHeaders()
    });
    
    const managerResponse = validateResponse<Manager>(managerRes.data);
    const manager = managerResponse.data;
    
    // Get restaurants by managerId and take first one
    const restaurantRes = await http.get<
      ApiSuccessResponse<Restaurant[]> | ApiErrorResponse
    >(`${API_URL}/Restaurants/manager/${manager.managerId}`, {
      headers: createAuthHeaders()
    });
    
    const restaurantResponse = validateResponse<Restaurant[]>(restaurantRes.data);
    const restaurants = restaurantResponse.data;
    
    if (!Array.isArray(restaurants) || restaurants.length === 0) {
      throw {
        message: 'No restaurant found for this manager',
        status: 404
      } as AppError;
    }
    
    return {
      success: true,
      data: restaurants[0],
      message: 'Restaurant found successfully'
    } as ApiSuccessResponse<Restaurant>;
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Menu endpoints
export async function getMenuItemsByRestaurant(restaurantId: number) {
  try {
    const res = await http.get<MenuItem[] | ApiSuccessResponse<MenuItem[]> | ApiErrorResponse>(
      `${API_URL}/MenuLists/restaurant/${restaurantId}`
    );

    return validateResponse<MenuItem[]>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getMenuItemById(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<MenuItem> | ApiErrorResponse
    >(`${API_URL}/MenuLists/${id}`);

    return validateResponse<MenuItem>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function createMenuItem(data: CreateMenuItem) {
  try {
    const formData = new FormData();
    formData.append('RestaurantId', data.restaurantId.toString());
    formData.append('ItemName', data.itemName);
    formData.append('Description', data.description);
    formData.append('AvailableQty', data.availableQty.toString());
    formData.append('Discount', data.discount.toString());
    formData.append('Price', data.price.toString());
    formData.append('IsVegetarian', data.isVegetarian.toString());
    formData.append('Tax', data.tax.toString());
    if (data.image) {
      formData.append('image', data.image);
    }

    const res = await http.post<
      ApiSuccessResponse<MenuItem> | ApiErrorResponse
    >(`${API_URL}/MenuLists`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    return validateResponse<MenuItem>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function updateMenuItem(id: number, data: UpdateMenuItem) {
  try {
    const formData = new FormData();
    
    if (data.itemName) formData.append('ItemName', data.itemName);
    if (data.description) formData.append('Description', data.description);
    if (data.availableQty !== undefined) formData.append('AvailableQty', data.availableQty.toString());
    if (data.discount !== undefined) formData.append('Discount', data.discount.toString());
    if (data.price !== undefined) formData.append('Price', data.price.toString());
    if (data.isVegetarian !== undefined) formData.append('IsVegetarian', data.isVegetarian.toString());
    if (data.tax !== undefined) formData.append('Tax', data.tax.toString());
    
    if (data.image) {
      formData.append('image', data.image);
    }

    const res = await http.patch<
      ApiSuccessResponse<MenuItem> | ApiErrorResponse
    >(`${API_URL}/MenuLists/${id}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    return validateResponse<MenuItem>(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function deleteMenuItem(id: number) {
  try {
    const res = await http.delete<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/MenuLists/${id}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTablesByRestaurant(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<DineInTable[]> | ApiErrorResponse
    >(`${API_URL}/DineIns/restaurant/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTableById(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<DineInTable> | ApiErrorResponse
    >(`${API_URL}/DineIns/${id}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function createTable(data: CreateDineInTable) {
  try {
    const res = await http.post<
      ApiSuccessResponse<DineInTable> | ApiErrorResponse
    >(`${API_URL}/DineIns`, data);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function updateTable(id: number, data: UpdateDineInTable) {
  try {
    const res = await http.patch<
      ApiSuccessResponse<DineInTable> | ApiErrorResponse
    >(`${API_URL}/DineIns/${id}`, data);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function deleteTable(id: number) {
  try {
    const res = await http.delete<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/DineIns/${id}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getAllOrderHistory() {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderHistory[]> | ApiErrorResponse
    >(`${API_URL}/OrderHistory`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderHistoryByRestaurant(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderHistory[]> | ApiErrorResponse
    >(`${API_URL}/OrderHistory/restaurant/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderHistoryByUser(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderHistory[]> | ApiErrorResponse
    >(`${API_URL}/OrderHistory/user/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderHistoryById(orderId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderHistory> | ApiErrorResponse
    >(`${API_URL}/OrderHistory/${orderId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function createOrderFromCart(userId: number) {
  try {
    const res = await http.post<
      ApiSuccessResponse<Order> | ApiErrorResponse
    >(`${API_URL}/Orders/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderDetails(id: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Order> | ApiErrorResponse
    >(`${API_URL}/Orders/${id}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getOrderSummaryByUser(userId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<OrderSummary> | ApiErrorResponse
    >(`${API_URL}/Orders/summary/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getRestaurantReviews(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Review[]> | ApiErrorResponse
    >(`${API_URL}/Review/restaurant/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getRestaurantRating(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<RestaurantRating> | ApiErrorResponse
    >(`${API_URL}/Review/rating/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getLatestReviews(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Review[]> | ApiErrorResponse
    >(`${API_URL}/Review/restaurant/${restaurantId}/latest`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getTopRatedReviews(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Review[]> | ApiErrorResponse
    >(`${API_URL}/Review/restaurant/${restaurantId}/top-rated`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getMenuItemReviews(itemId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<Review[]> | ApiErrorResponse
    >(`${API_URL}/Review/item/${itemId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getMenuItemRating(itemId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<MenuItemRating> | ApiErrorResponse
    >(`${API_URL}/Review/item/${itemId}/rating`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

// Admin Payout endpoints
export async function getRestaurantRevenue(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<RestaurantRevenue> | ApiErrorResponse
    >(`${API_URL}/Admin/analytics/restaurant/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getPayoutHistory(managerId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<PayoutHistory[]> | ApiErrorResponse
    >(`${API_URL}/AdminManager/payout-history/${managerId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function processPayout(payoutData: PayoutData) {
  try {
    const res = await http.post<
      ApiSuccessResponse<PayoutHistory> | ApiErrorResponse
    >(`${API_URL}/AdminManager/payout`, payoutData);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}