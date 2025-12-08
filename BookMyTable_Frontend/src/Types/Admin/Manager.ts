export interface Manager {
  managerId: number;
  managerName: string;
  userId: number;
  email: string;
  phoneNumber: string;
  passwordHash: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
  verification: number;
  restaurants: [];
}

export interface AdminRestaurant {
  restaurantId: number;
  restaurantName: string;
  images?: string;
  description: string;
  ratings: number;
  restaurantCategory: number;
  restaurantType: number;
  location: string;
  city: string;
  contactNo: string;
  deliveryCharge: number;
  managerId: number;
  manager: Manager;
  isActive: boolean;
  createdAt: string;
}
