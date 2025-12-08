import { createAsyncThunk } from "@reduxjs/toolkit";
import { getCustomersByRestaurant, getCustomerSummary, getOrdersByRestaurant, getTodayOrders, getOrderSummary, getPaymentsByRestaurant, getPaymentSummary, getRestaurantById, updateRestaurant as updateRestaurantApi } from "../../Service/manager.api";
import { handleAxiosError } from "../../Service/handleApiError";
import type { Customer, CustomerSummary } from "../../Types/Manager/Customer";
import type { Order, OrderSummary } from "../../Types/Manager/Order";
import type { Payment, PaymentSummary } from "../../Types/Manager/Payment";
import type { Restaurant, UpdateRestaurant } from "../../Types/Manager/Restaurant";

// Customer thunks
export const fetchCustomersByRestaurant = createAsyncThunk<
  Customer[],
  number,
  { rejectValue: string }
>(
  "manager/fetchCustomersByRestaurant",
  async (restaurantId, { rejectWithValue }) => {
    try {
      const customers = await getCustomersByRestaurant(restaurantId);
      return customers.data as Customer[];
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

export const fetchCustomerSummary = createAsyncThunk<
  CustomerSummary,
  number,
  { rejectValue: string }
>(
  "manager/fetchCustomerSummary",
  async (restaurantId, { rejectWithValue }) => {
    try {
      const summary = await getCustomerSummary(restaurantId);
      return summary.data as CustomerSummary;
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

// Order thunks
export const fetchOrdersByRestaurant = createAsyncThunk<
  Order[],
  { restaurantId: number; startDate?: string; endDate?: string; status?: string },
  { rejectValue: string }
>(
  "manager/fetchOrdersByRestaurant",
  async ({ restaurantId, startDate, endDate, status }, { rejectWithValue }) => {
    try {
      const orders = await getOrdersByRestaurant(restaurantId, startDate, endDate, status);
      return orders.data as Order[];
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

export const fetchTodayOrders = createAsyncThunk<
  Order[],
  number,
  { rejectValue: string }
>(
  "manager/fetchTodayOrders",
  async (restaurantId, { rejectWithValue }) => {
    try {
      const orders = await getTodayOrders(restaurantId);
      return orders.data as Order[];
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

export const fetchOrderSummary = createAsyncThunk<
  OrderSummary,
  { restaurantId: number; startDate?: string; endDate?: string },
  { rejectValue: string }
>(
  "manager/fetchOrderSummary",
  async ({ restaurantId, startDate, endDate }, { rejectWithValue }) => {
    try {
      const summary = await getOrderSummary(restaurantId, startDate, endDate);
      return summary.data as OrderSummary;
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

// Payment thunks
export const fetchPaymentsByRestaurant = createAsyncThunk<
  Payment[],
  { restaurantId: number; startDate?: string; endDate?: string; paymentMethod?: string },
  { rejectValue: string }
>(
  "manager/fetchPaymentsByRestaurant",
  async ({ restaurantId, startDate, endDate, paymentMethod }, { rejectWithValue }) => {
    try {
      const payments = await getPaymentsByRestaurant(restaurantId, startDate, endDate, paymentMethod);
      return payments.data as Payment[];
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

export const fetchPaymentSummary = createAsyncThunk<
  PaymentSummary,
  { restaurantId: number; startDate?: string; endDate?: string },
  { rejectValue: string }
>(
  "manager/fetchPaymentSummary",
  async ({ restaurantId, startDate, endDate }, { rejectWithValue }) => {
    try {
      const summary = await getPaymentSummary(restaurantId, startDate, endDate);
      return summary.data as PaymentSummary;
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

// Restaurant thunks
export const fetchRestaurantById = createAsyncThunk<
  Restaurant,
  number,
  { rejectValue: string }
>(
  "manager/fetchRestaurantById",
  async (id, { rejectWithValue }) => {
    try {
      const restaurant = await getRestaurantById(id);
      return restaurant.data as Restaurant;
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);

export const updateRestaurant = createAsyncThunk<
  Restaurant,
  { id: number; data: UpdateRestaurant },
  { rejectValue: string }
>(
  "manager/updateRestaurant",
  async ({ id, data }, { rejectWithValue }) => {
    try {
      const restaurant = await updateRestaurantApi(id, data);
      return restaurant.data as Restaurant;
    } catch (error) {
      const axiosError = handleAxiosError(error);
      return rejectWithValue(axiosError.message);
    }
  }
);