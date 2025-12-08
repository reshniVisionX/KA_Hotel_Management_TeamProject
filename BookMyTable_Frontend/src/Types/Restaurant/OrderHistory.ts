export interface OrderHistory {
  orderId: number;
  restaurantId: number;
  userId: number;
  itemsList: string;
  orderType: number;
  qtyOrdered: number;
  orderDate: string;
  totalAmount: number;
  status: number;
  userName: string;
}
