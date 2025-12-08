export interface DineInTable {
  id: number;
  restaurantId: number;
  tableNumber: string;
  capacity: number;
  isAvailable: boolean;
  location?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateDineInTable {
  restaurantId: number;
  tableNumber: string;
  capacity: number;
  location?: string;
}

export interface UpdateDineInTable {
  tableNumber?: string;
  capacity?: number;
  isAvailable?: boolean;
  location?: string;
}

export interface OrderHistory {
  orderId: number;
  customerId: number;
  restaurantId: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  items: OrderHistoryItem[];
  createdAt: string;
}

export interface OrderHistoryItem {
  itemId: number;
  itemName: string;
  quantity: number;
  price: number;
  total: number;
}

export interface Review {
  reviewId: number;
  customerId: number;
  restaurantId: number;
  menuItemId?: number;
  rating: number;
  comment?: string;
  createdAt: string;
  customerName: string;
}

export interface RestaurantRating {
  restaurantId: number;
  averageRating: number;
  totalReviews: number;
  ratingDistribution: {
    [key: number]: number;
  };
}

export interface MenuItemRating {
  menuItemId: number;
  averageRating: number;
  totalReviews: number;
}

export interface PayoutData {
  managerId: number;
  restaurantId: number;
  amount: number;
  remarks?: string;
}

export interface PayoutHistory {
  payoutId: number;
  managerId: number;
  restaurantId: number;
  amount: number;
  payoutDate: string;
  status: string;
  remarks?: string;
}

export interface RestaurantRevenue {
  restaurantId: number;
  totalRevenue: number;
  monthlyRevenue: number;
  dailyRevenue: number;
  orderCount: number;
  averageOrderValue: number;
}