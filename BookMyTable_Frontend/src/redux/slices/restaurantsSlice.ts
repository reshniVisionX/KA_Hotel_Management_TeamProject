import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { Restaurant } from "../../Types/Restaurant/Restaurants";
import { fetchAllRestaurants } from "../thunks/restaurantThunk";

interface RestaurantsState {
  restaurants: Restaurant[];
  loading: boolean;
  error: string | null;
}

const initialState: RestaurantsState = {
  restaurants: [],
  loading: false,
  error: null,
};

const restaurantsSlice = createSlice({
  name: "restaurants",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllRestaurants.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(
        fetchAllRestaurants.fulfilled,
        (state, action: PayloadAction<Restaurant[]>) => {
          state.loading = false;
          state.restaurants = action.payload;
        }
      )
      .addCase(fetchAllRestaurants.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to fetch restaurants";
      });
  },
});

export const { clearError } = restaurantsSlice.actions;
export default restaurantsSlice.reducer;
