export interface MenuItem {
  itemId: number;
  restaurantId: number;
  itemName: string;
  description: string;
  availableQty: number;
  discount: number;
  price: number;
  isVegetarian: boolean;
  tax: number;
  image?: string | null;
  isLiked?: boolean;
}
