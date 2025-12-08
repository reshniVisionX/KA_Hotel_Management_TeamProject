export interface Order {
  orderId: number;
  restaurantId: number;
  items: ItemQuantity[];
  orderType: OrderType;
  userId: number;
  qtyOrdered: number;
  orderDate: string;
  totalAmount: number;
  status: OrderStatus;
}

export interface ItemQuantity {
  itemId: number;
  quantity: number;
}

export const OrderType = {
  DineIn: 0,
  Delivery: 1,
  Takeaway: 2
} as const;

export type OrderType = typeof OrderType[keyof typeof OrderType];

export const OrderStatus = {
  Pending: 0,
  Confirmed: 1,
  Preparing: 2,
  Ready: 3,
  Delivered: 4,
  Cancelled: 5
} as const;

export type OrderStatus = typeof OrderStatus[keyof typeof OrderStatus];

export interface OrderSummary {
  totalOrders: number;
  todayOrders: number;
  pendingOrders: number;
  completedOrders: number;
  totalRevenue: number;
  todayRevenue: number;
}

export interface DailyRevenue {
  date: string;
  revenue: number;
}