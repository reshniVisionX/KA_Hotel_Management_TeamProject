export type OrderStatusType = typeof OrderStatus[keyof typeof OrderStatus];
export type OrderTypeType = typeof OrderType[keyof typeof OrderType];

export interface CartItem {
  cartId: number;
  itemId: number;
  restaurantId: number;
  itemName: string;
  restaurantName: string;
  price: number;
  quantity: number;
  totalPrice: number;
  isVegetarian: boolean;
  addedAt: string;
}

export interface CartResponse {
  items: CartItem[];
  grandTotal: number;
  totalItems: number;
}

export const OrderStatus = {
  Pending: 0,
  InProgress: 1,
  Completed: 2,
  Cancelled: 3
} as const;

export const OrderType = {
  DineIn: 0,
  DineOut: 1,
  TakeAway: 2
} as const;

export const PayMethod = {
  Cash: 0,
  Card: 1,
  UPI: 2,
  Wallet: 3
} 

export type PayMethodType = typeof PayMethod[keyof typeof PayMethod];

export interface OrderItem {
  itemId: number;
  quantity: number;
}

export interface OrderResponse {
  orderId: number;
  restaurantId: number;
  itemsList: string;
  items: OrderItem[];
  orderType: OrderTypeType;
  userId: number;
  qtyOrdered: number;
  orderDate: string;
  totalAmount: number;
  status: OrderStatusType;
}

export interface OrderSummaryItem {
  itemId: number;
  itemName: string;
  originalPrice: number;
  discount: number;
  priceAfterDiscount: number;
  taxPercentage: number;
  quantity: number;
  itemSubtotal: number;
  itemDiscount: number;
  itemAfterDiscount: number;
  itemTax: number;
  itemTotal: number;
}

export interface OrderSummary {
  orderId: number;
  restaurantId: number;
  restaurantName: string;
  orderDate: string;
  items: OrderSummaryItem[];
  totalQuantity: number;
  subtotal: number;
  totalDiscount: number;
  totalAfterDiscount: number;
  totalTax: number;
  grandTotal: number;
}

export interface PaymentRequest {
  orderId: number;
  payMethod: number;
}

export interface PaymentResponse {
  paymentId: number;
  orderId: number;
  amount: number;
  paymentDate: string;
  payMethod: number;
  status: number;
  order: OrderResponse;
}
