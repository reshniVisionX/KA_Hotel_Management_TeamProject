import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { MenuItem } from "../../Types/Restaurant/MenuItems";
import { fetchAllMenuItems } from "../thunks/menuThunk";

interface MenuState {
  menuItems: MenuItem[];
  loading: boolean;
  error: string | null;
}

const initialState: MenuState = {
  menuItems: [],
  loading: false,
  error: null,
};

const menuSlice = createSlice({
  name: "menu",
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    updateMenuItemLike: (state, action: PayloadAction<{ itemId: number; isLiked: boolean }>) => {
      const item = state.menuItems.find(item => item.itemId === action.payload.itemId);
      if (item) {
        item.isLiked = action.payload.isLiked;
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchAllMenuItems.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(
        fetchAllMenuItems.fulfilled,
        (state, action: PayloadAction<MenuItem[]>) => {
          state.loading = false;
          state.menuItems = action.payload;
        }
      )
      .addCase(fetchAllMenuItems.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || "Failed to fetch menu items";
      });
  },
});

export const { clearError, updateMenuItemLike } = menuSlice.actions;
export default menuSlice.reducer;
