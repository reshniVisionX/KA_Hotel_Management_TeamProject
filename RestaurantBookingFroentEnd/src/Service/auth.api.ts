import axios from "axios";
import type { LoginResponse } from "../Types/Dashboard/LoginResponse";
import type { ApiSuccessResponse } from "../Types/Dashboard/ApiSuccessResponse";
import type { ApiErrorResponse } from "../Types/Dashboard/ApiErrorResponse";
import type { OtpResponse } from "../Types/Dashboard/OtpResponse";  
import type { Login } from "../Types/Dashboard/Login";
import type { Register } from "../Types/Dashboard/Register";

const API_URL = import.meta.env.VITE_API_URL || "";  

export async function loginApi(credentials: Login) {
  return axios.post<ApiSuccessResponse<LoginResponse> | ApiErrorResponse>(
    `${API_URL}/Users/login`,
    credentials
  );
}

export async function generateOtpApi(mobileNo: string) {
  return axios.post<OtpResponse>(
    `${API_URL}/Users/generate-otp`,
    { mobileNo }
  );
}

export async function registerApi(payload: Register) {
  return axios.post<ApiSuccessResponse<null> | ApiErrorResponse>(
    `${API_URL}/Users/register`,
    payload
  );
}