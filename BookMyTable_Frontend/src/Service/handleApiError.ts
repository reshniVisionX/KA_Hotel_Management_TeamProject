import axios from "axios";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { AppError } from "../Types/Dashboard/ApiError";

export function handleAxiosError(error: unknown): AppError {

  // CASE 1: validateResponse() threw an AppError manually
  if (typeof error === "object" && error !== null && "message" in error) {
    const err = error as AppError;
    return {
      message: err.message,
      status: err.status ?? 400
    };
  }

  // CASE 2: Axios error (network/server/HTTP)
  if (axios.isAxiosError(error)) {
    const apiError = error.response?.data as ApiErrorResponse | undefined;

    return {
      message: apiError?.message || error.message || "Unknown server error",
      status: error.response?.status ?? 500
    };
  }

  // CASE 3: Unexpected non-Axios error
  return {
    message: "Unexpected error occurred",
    status: 500
  };
}
