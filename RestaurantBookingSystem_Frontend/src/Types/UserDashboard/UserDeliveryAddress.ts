export interface UserDeliveryAddress {
  id: string;
  addressType: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  phoneNumber: string;
  isDefault: boolean;
}

export interface UserDeliveryAddressResponse {
  addresses: UserDeliveryAddress[];
  total: number;
}

export interface CreateDeliveryAddressRequest {
  userId: number;
  addressType: string;
  street: string;
  city: string;
  state: string;
  zipCode: string;
  phoneNumber: string;
  isDefault: boolean;
}