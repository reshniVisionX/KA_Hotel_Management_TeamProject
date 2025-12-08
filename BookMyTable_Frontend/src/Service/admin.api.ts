import {http} from "./https";
import { handleAxiosError } from "./handleApiError";
import type { ApiSuccessResponse } from "../Types/Dashboard/ApiSuccessResponse";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { AppError } from "../Types/Dashboard/ApiError";
import type { AdminRestaurant } from "../Types/Admin/Manager";
import type { AdminManager } from "../Types/Admin/AdminManager";
import type { DashboardAnalytics, RestaurantAnalytics } from "../Types/Admin/Analytics";
import type { CreateDeliveryPersonRequest, DeliveryPersonResponse } from "../Types/Admin/DeliveryPerson";

const API_URL = import.meta.env.VITE_API_URL || "";

function validateResponse<T>(
  response: ApiSuccessResponse<T> | ApiErrorResponse
): ApiSuccessResponse<T> {
  if (response.success === true) {
    return response;
  }
  throw {
    message: response.message,
    status: 400
  } as AppError;
}

export async function getAllRestaurants() {
  try {
    const res = await http.get<
      ApiSuccessResponse<AdminRestaurant[]> | ApiErrorResponse
    >(`${API_URL}/Admin/restaurants`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getAllManagers() {
  try {
    const res = await http.get<
      ApiSuccessResponse<AdminManager[]> | ApiErrorResponse
    >(`${API_URL}/Admin/managers`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function toggleRestaurantStatus(restaurantId: number) {
  try {
    const res = await http.put<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Admin/restaurants/${restaurantId}/toggle`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function toggleManagerStatus(managerId: number) {
  try {
    const res = await http.put<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Admin/managers/${managerId}/toggle`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getDashboardAnalytics() {
  try {
    const res = await http.get<
      ApiSuccessResponse<DashboardAnalytics> | ApiErrorResponse
    >(`${API_URL}/Admin/analytics/dashboard`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function getRestaurantAnalytics(restaurantId: number) {
  try {
    const res = await http.get<
      ApiSuccessResponse<RestaurantAnalytics[]> | ApiErrorResponse
    >(`${API_URL}/Admin/analytics/restaurant/${restaurantId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export interface UpdateVerificationRequest {
  managerId: number;
  verificationStatus: number;
}

export async function updateManagerVerification(payload: UpdateVerificationRequest) {
  try {
    const res = await http.put<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/Admin/update-verification`, payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}


export async function addDeliveryPerson(payload: CreateDeliveryPersonRequest) {
  try {
    const res = await http.post<
      ApiSuccessResponse<DeliveryPersonResponse> | ApiErrorResponse
    >("/DeliveryPerson", payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}
