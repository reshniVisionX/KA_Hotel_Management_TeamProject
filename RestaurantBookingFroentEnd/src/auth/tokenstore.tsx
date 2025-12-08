
import type { AuthUser } from "../Types/Dashboard/AuthUser";

const TOKEN_KEY = "bookmytable_auth_token";
const CUSTOMER_KEY = "bookmytable_user";
const THEME_KEY = "bookmytable_theme";

export const tokenstore = {
  getToken() {
    return localStorage.getItem(TOKEN_KEY) || null;
  },

  setToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  },

  setUser(user: AuthUser) {
    localStorage.setItem(CUSTOMER_KEY, JSON.stringify(user));
  },

  getUser(): AuthUser | null {
    const data = localStorage.getItem(CUSTOMER_KEY);
    return data ? JSON.parse(data) : null;
  },

  setTheme(theme: "light" | "dark") {
    localStorage.setItem(THEME_KEY, theme);
  },

  getTheme(): "light" | "dark" {
    return (localStorage.getItem(THEME_KEY) as "light" | "dark") || "light";
  },

  clear() {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(CUSTOMER_KEY);
  },
};
