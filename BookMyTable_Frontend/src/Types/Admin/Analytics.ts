export interface DashboardAnalytics {
  noOfRestaurants: number;
  noOfUsers: number;
  noOfManagers: number;
  noOfReservations: number;
  noOfActiveUsers: number;
  noOfActiveReservations: number;
  noOfActiveManagers: number;
  dineInOrders: number;
  dineOutOrders: number;
  noOfVegetarianHotels: number;
  noOfNonVegetarianHotels: number;
}

export interface RestaurantAnalytics {
  restaurantId: number;
  restaurantName: string;
  dailyRevenue: number;
  weeklyRevenue: number;
  noOfDailyOrders: number;
  weeklyOrders: number;
  monthlyOrders: number;
  monthlyRevenue: number;
}
