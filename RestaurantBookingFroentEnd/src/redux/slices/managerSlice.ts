import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { Customer, CustomerSummary } from "../../Types/Manager/Customer";
import type { Order, OrderSummary } from "../../Types/Manager/Order";
import type { Payment, PaymentSummary } from "../../Types/Manager/Payment";
import type { Restaurant } from "../../Types/Manager/Restaurant";
import {
  fetchCustomersByRestaurant,
  fetchCustomerSummary,
  fetchOrdersByRestaurant,
  fetchTodayOrders,
  fetchOrderSummary,
  fetchPaymentsByRestaurant,
  fetchPaymentSummary,
  fetchRestaurantById,
  updateRestaurant
} from "../thunks/managerThunk";

interface ManagerState {
  // Customer data
  customers: Customer[];
  customerSummary: CustomerSummary | null;
  
  // Order data
  orders: Order[];
  todayOrders: Order[];
  orderSummary: OrderSummary | null;
  
  // Payment data
  payments: Payment[];
  paymentSummary: PaymentSummary | null;
  
  // Restaurant data
  restaurant: Restaurant | null;
  
  // Loading states
  loading: {
    customers: boolean;
    orders: boolean;
    payments: boolean;
    restaurant: boolean;
  };
  
  // Error states
  error: string | null;
}

const initialState: ManagerState = {
  customers: [],
  customerSummary: null,
  orders: [],
  todayOrders: [],
  orderSummary: null,
  payments: [],
  paymentSummary: null,
  restaurant: null,
  loading: {
    customers: false,
    orders: false,
    payments: false,
    restaurant: false,
  },
  error: null,
};

const managerSlice = createSlice({
  name: "manager",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    clearManagerData: (state) => {
      state.customers = [];
      state.customerSummary = null;
      state.orders = [];
      state.todayOrders = [];
      state.orderSummary = null;
      state.payments = [];
      state.paymentSummary = null;
      state.restaurant = null;
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // Customer cases
      .addCase(fetchCustomersByRestaurant.pending, (state) => {
        state.loading.customers = true;
        state.error = null;
      })
      .addCase(fetchCustomersByRestaurant.fulfilled, (state, action: PayloadAction<Customer[]>) => {
        state.loading.customers = false;
        state.customers = action.payload;
      })
      .addCase(fetchCustomersByRestaurant.rejected, (state, action) => {
        state.loading.customers = false;
        state.error = action.payload as string;
      })
      
      .addCase(fetchCustomerSummary.pending, (state) => {
        state.loading.customers = true;
      })
      .addCase(fetchCustomerSummary.fulfilled, (state, action: PayloadAction<CustomerSummary>) => {
        state.loading.customers = false;
        state.customerSummary = action.payload;
      })
      .addCase(fetchCustomerSummary.rejected, (state, action) => {
        state.loading.customers = false;
        state.error = action.payload as string;
      })
      
      // Order cases
      .addCase(fetchOrdersByRestaurant.pending, (state) => {
        state.loading.orders = true;
        state.error = null;
      })
      .addCase(fetchOrdersByRestaurant.fulfilled, (state, action: PayloadAction<Order[]>) => {
        state.loading.orders = false;
        state.orders = action.payload;
      })
      .addCase(fetchOrdersByRestaurant.rejected, (state, action) => {
        state.loading.orders = false;
        state.error = action.payload as string;
      })
      
      .addCase(fetchTodayOrders.fulfilled, (state, action: PayloadAction<Order[]>) => {
        state.todayOrders = action.payload;
      })
      
      .addCase(fetchOrderSummary.fulfilled, (state, action: PayloadAction<OrderSummary>) => {
        state.orderSummary = action.payload;
      })
      
      // Payment cases
      .addCase(fetchPaymentsByRestaurant.pending, (state) => {
        state.loading.payments = true;
        state.error = null;
      })
      .addCase(fetchPaymentsByRestaurant.fulfilled, (state, action: PayloadAction<Payment[]>) => {
        state.loading.payments = false;
        state.payments = action.payload;
      })
      .addCase(fetchPaymentsByRestaurant.rejected, (state, action) => {
        state.loading.payments = false;
        state.error = action.payload as string;
      })
      
      .addCase(fetchPaymentSummary.fulfilled, (state, action: PayloadAction<PaymentSummary>) => {
        state.paymentSummary = action.payload;
      })
      
      // Restaurant cases
      .addCase(fetchRestaurantById.pending, (state) => {
        state.loading.restaurant = true;
        state.error = null;
      })
      .addCase(fetchRestaurantById.fulfilled, (state, action: PayloadAction<Restaurant>) => {
        state.loading.restaurant = false;
        state.restaurant = action.payload;
      })
      .addCase(fetchRestaurantById.rejected, (state, action) => {
        state.loading.restaurant = false;
        state.error = action.payload as string;
      })
      
      .addCase(updateRestaurant.fulfilled, (state, action: PayloadAction<Restaurant>) => {
        state.restaurant = action.payload;
      });
  },
});

export const { clearError, clearManagerData } = managerSlice.actions;
export default managerSlice.reducer;