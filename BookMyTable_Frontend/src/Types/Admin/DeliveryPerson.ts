export interface CreateDeliveryPersonRequest {
  deliveryName: string;
  mobileNo: string;
  email: string;
  licenseNumber: string;
}

export interface DeliveryPersonResponse {
  deliveryPersonId: number;
  deliveryName: string;
  mobileNo: string;
  email: string;
  licenseNumber: string;
  status: number;
  totalDeliveries: number;
  averageRating: number;
}
