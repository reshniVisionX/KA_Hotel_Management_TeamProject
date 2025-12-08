import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import { loginUser } from "../thunks/authThunk";
import type { LoginResponse } from "../../Types/Dashboard/LoginResponse";
import { tokenstore } from "../../auth/tokenstore";
import type { AuthUser } from "../../Types/Dashboard/AuthUser";

type AuthState = {
  user: AuthUser | null;
  loading: boolean;
  error: string | null;
};

const initialState: AuthState = {
  user: tokenstore.getUser(),
  loading: false,
  error: null,
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    logout(state) {
      state.user = null;
      tokenstore.clear();
    },

    setCity(state, action: PayloadAction<string>) {
      if (state.user) {
        state.user.city = action.payload;
      }
    },

  },
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(
        loginUser.fulfilled,
        (state, action: PayloadAction<LoginResponse>) => {
          state.loading = false;
          state.user = action.payload;
        }
      )
      .addCase(loginUser.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        console.log("Login Error:", action.payload);
      });

  },
});

export const { logout, setCity } = authSlice.actions;
export default authSlice.reducer;
