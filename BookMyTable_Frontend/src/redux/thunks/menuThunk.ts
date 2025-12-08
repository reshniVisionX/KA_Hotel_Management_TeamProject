import { createAsyncThunk } from "@reduxjs/toolkit";
import type { MenuItem } from "../../Types/Restaurant/MenuItems";
import { getAllMenuItems, getUserWishlist } from "../../Service/restaurant.api";
import { handleAxiosError } from "../../Service/handleApiError";
import type { WishlistItem } from "../../Types/Restaurant/Wishlist";

export const fetchAllMenuItems = createAsyncThunk<
  MenuItem[],
  number | undefined, 
  { rejectValue: string }
>("menu/fetchAll", async (userId, { rejectWithValue }) => {
  try {
    const menuResponse = await getAllMenuItems();
    let wishlistItems: WishlistItem[] = [];

    if (userId) {
      try {
        const wishlistResponse = await getUserWishlist(userId);
        wishlistItems = wishlistResponse.data;
      } catch (error) {
        // If wishlist fetch fails, continue with empty wishlist
        console.log("Failed to fetch wishlist:", error);
        wishlistItems = [];
      }
    }

    const menuItems = menuResponse.data;

    // Add isLiked field to each menu item based on wishlist
    const menuItemsWithLikes = menuItems.map(item => ({
      ...item,
      isLiked: wishlistItems.some(wishItem => wishItem.itemId === item.itemId)
    }));

    return menuItemsWithLikes;
  } catch (err) {
    const appError = handleAxiosError(err);
    return rejectWithValue(appError.message);
  }
});
