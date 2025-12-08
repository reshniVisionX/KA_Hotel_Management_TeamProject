import { configureStore } from "@reduxjs/toolkit";
import authReducer from "./slices/authSlice";
import restaurantsReducer from "./slices/restaurantsSlice";
import menuReducer from "./slices/menuSlice";

export const store = configureStore({
  reducer: {
    auth: authReducer,
    restaurants: restaurantsReducer,
     menu: menuReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
