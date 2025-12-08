import { createAsyncThunk } from "@reduxjs/toolkit";
import type { Restaurant } from "../../Types/Restaurant/Restaurants";
import { getAllRestaurants } from "../../Service/restaurant.api";
import { handleAxiosError } from "../../Service/handleApiError";

export const fetchAllRestaurants = createAsyncThunk<
  Restaurant[],
  void,
  { rejectValue: string }
>("restaurants/fetchAll", async (_, { rejectWithValue }) => {
  try {
    const response = await getAllRestaurants();
    return response.data;
  } catch (err) {
    const appError = handleAxiosError(err);
    return rejectWithValue(appError.message);
  }
});
