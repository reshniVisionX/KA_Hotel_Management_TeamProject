export interface DeliveryAddress {
  addressId: number;
  userId: number;

  mobile: string;
  address: string;
  city: string;
  state: string;
  pincode: string;

  landmark?: string | null;
  contactNo?: string | null;

  isDefault: boolean;
}
