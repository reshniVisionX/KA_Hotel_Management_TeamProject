export interface WishlistItem {
  wishlistId: number;
  restaurantId: string;
  restaurantName: string;
  restaurantImage: string;
  cuisine: string;
  rating: number;
  addedAt: string;
  location: string;
  itemName: string;
  itemDescription: string;
  itemPrice: number;
  itemImage: string;
  isVegetarian: boolean;
}

export interface WishlistResponse {
  items: WishlistItem[];
  total: number;
}

export interface CreateWishlistRequest {
  userId: number;
  itemId: number;
}