import axios from "axios";
import { handleAxiosError } from "../Service/handleApiError";

import type { ApiSuccessResponse } from "../Types/Dashboard/ApiSuccessResponse";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { DeliveryAddress } from "../Types/Restaurant/DeliveryAddress";
import type { CreateDeliveryAddress } from "../Types/Restaurant/CreateDeliveryAddress";
import type { ChangeDeliveryAddress } from "../Types/Restaurant/ChangeDeliveryAddress";
import type { AppError } from "../Types/Dashboard/ApiError";

const API_URL = import.meta.env.VITE_API_URL || "";

// Helper to validate backend response
function validateResponse<T>(
  response: ApiSuccessResponse<T> | ApiErrorResponse
): ApiSuccessResponse<T> {
  if (response.success === true) {
    return response; // success: keep it
  }
  // error: throw a typed AppError
  throw {
    message: response.message,
    status: 400
  } as AppError;
}

export async function getDeliveryAddresses(userId: number) {
  try {
    const res = await axios.get<
      ApiSuccessResponse<DeliveryAddress[]> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/forUser/${userId}`);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function createDeliveryAddress(payload: CreateDeliveryAddress) {
  try {
    const res = await axios.post<
      ApiSuccessResponse<DeliveryAddress> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/createAddress`, payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}

export async function changeDefaultAddress(payload: ChangeDeliveryAddress) {
  try {
    const res = await axios.post<
      ApiSuccessResponse<null> | ApiErrorResponse
    >(`${API_URL}/delivery/AdminDelivery/changeDefault`, payload);

    return validateResponse(res.data);
  } catch (error) {
    throw handleAxiosError(error);
  }
}
