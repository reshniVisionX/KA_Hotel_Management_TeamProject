export interface WishlistItem {
  wishlistId: number;
  itemId: number;
  restaurantId: number;
  itemName: string;
  restaurantName: string;
  price: number;
  isVegetarian: boolean;
  createdAt: string;
}

export interface AddToWishlistRequest {
  itemId: number;
  restaurantId: number;
}
