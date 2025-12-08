export interface DeliveryPersonHistory {
   // Delivery Info
  deliveryId: number | null;
  deliveryStatus: number | null;
  estimatedDeliveryTime: string | null;
  // delivery Address info
  mobile: string | null;
  address: string | null;
  city: string | null;
  state: string | null;
  pincode: string | null;
  landmark: string | null;
  // customer details
  customerFirstName: string | null;
  customerLastName: string | null;
  // delivery person info
  deliveryPersonId: number;
  deliveryName: string;
  licenseNumber: string;
  status: number;
  totalDeliveries: number;
  averageRating: number;
}

export interface CompleteDeliveryResponse {
  deliveryId: number;
}
