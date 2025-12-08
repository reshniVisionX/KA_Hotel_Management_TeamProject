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
  image?: string; // Base64 string for display
}

export interface CreateMenuItem {
  restaurantId: number;
  itemName: string;
  description: string;
  availableQty: number;
  discount: number;
  price: number;
  isVegetarian: boolean;
  tax: number;
  image?: File;
}

export interface UpdateMenuItem {
  itemName?: string;
  description?: string;
  availableQty?: number;
  discount?: number;
  price?: number;
  isVegetarian?: boolean;
  tax?: number;
  image?: File;
}