import axios from "axios";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { AppError } from "../Types/Dashboard/ApiError";

export function handleAxiosError(error: unknown): AppError {
  if (axios.isAxiosError(error)) {
    const apiError = error.response?.data as ApiErrorResponse | undefined;

    return {
      message: apiError?.message || error.message || "Unknown server error",
      status: error.response?.status
    };
  }

  return { message: "Unexpected error occurred" };
}
