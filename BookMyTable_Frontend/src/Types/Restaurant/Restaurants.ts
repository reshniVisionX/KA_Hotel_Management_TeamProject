import type { MenuItem } from "./MenuItems";

export interface Restaurant {
  restaurantId: number;
  restaurantName: string;
  description: string | null;
  ratings: number | null;
  restaurantCategory: number;
  restaurantType: number;
  location: string;
  city: string;
  contactNo: string;
  deliveryCharge: number | null;
  managerId: number;
  isActive: boolean;
  menuLists: MenuItem[];
  createdAt: string;
  images?: string | null;
}
