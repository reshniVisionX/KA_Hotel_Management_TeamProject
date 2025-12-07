import { createAsyncThunk } from "@reduxjs/toolkit";
import type { LoginResponse } from "../../Types/Dashboard/LoginResponse";
import { tokenstore } from "../../auth/tokenstore";
import { loginApi } from "../../Service/auth.api";
import type { ApiErrorResponse } from "../../Types/Dashboard/ApiErrorResponse";
import { handleAxiosError } from "../../Service/handleApiError";
import type { Login } from "../../Types/Dashboard/Login";

type Credentials = Login;

export const loginUser = createAsyncThunk<
  LoginResponse,
  Credentials,
  { rejectValue: string }
>("auth/login", async (credentials, { rejectWithValue }) => {
  try {
    const response = await loginApi(credentials);
    const body = response.data ;

    if (body.success==true) {
      const user = body.data;

      tokenstore.setUser({
        userId: user.userId,
        roleName: user.roleName,
        firstName: user.firstName,
        token: user.token,
      });

      tokenstore.setToken(user.token);

      return user;
    }else{
      console.log("Login failed:", body.success);
       return rejectWithValue((body as ApiErrorResponse).message);
    }
  } catch (err) {
    const appError = handleAxiosError(err);
    return rejectWithValue(appError.message);
  }
});
