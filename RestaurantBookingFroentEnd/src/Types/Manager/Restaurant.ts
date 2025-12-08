export interface Manager {
  managerId: number;
  managerName: string;
  userId: number;
  email: string;
  phoneNumber: string;
  isActive: boolean;
}

export interface Restaurant {
  restaurantId: number;
  restaurantName: string;
  images?: Uint8Array;
  description: string;
  ratings?: number;
  restaurantCategory: RestaurantCategory;
  restaurantType: FoodType;
  location: string;
  city: string;
  contactNo: string;
  deliveryCharge?: number;
  managerId: number;
  manager?: Manager;
  isActive: boolean;
  createdAt: string;
}

export const RestaurantCategory = {
  FastFood: 0,
  Casual: 1,
  Finedining: 2,
  Cafe: 3,
  Buffet: 4
} as const;

export type RestaurantCategory = typeof RestaurantCategory[keyof typeof RestaurantCategory];

export const FoodType = {
  Veg: 0,
  NonVeg: 1,
  Both: 2
} as const;

export type FoodType = typeof FoodType[keyof typeof FoodType];

export interface UpdateRestaurant {
  restaurantName?: string;
  description?: string;
  location?: string;
  contactNo?: string;
  deliveryCharge?: number;
}