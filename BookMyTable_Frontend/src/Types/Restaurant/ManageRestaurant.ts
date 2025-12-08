export interface ManagerRegisterRequest {
  // Manager fields
  managerName: string;
  userId: number;
  email: string;
  phoneNumber: string;
  password: string;

  // Restaurant fields
  restaurantName: string;
  description?: string;
  ratings?: number;
  restaurantCategory: number;
  restaurantType: number;
  location: string;
  city: string;
  contactNo: string;
  deliveryCharge?: number;
  isActive: boolean;

  // File
  image?: File;
}
