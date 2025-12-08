export interface OrderItem {
  id: string;
  name: string;
  quantity: number;
  price: number;
}

export interface Order {
  id: string;
  restaurantName: string;
  restaurantImage?: string;
  items: OrderItem[];
  totalAmount: number;
  status: 'pending' | 'confirmed' | 'preparing' | 'delivered' | 'cancelled';
  orderDate: string;
  deliveryAddress: string;
}

export interface OrdersResponse {
  orders: Order[];
  total: number;
}