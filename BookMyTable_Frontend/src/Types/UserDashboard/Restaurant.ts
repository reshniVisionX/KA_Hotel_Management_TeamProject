export interface Restaurant {
  id: string;
  name: string;
  description: string;
  cuisine: string;
  status: 'active' | 'inactive' | 'pending';
}

export interface RestaurantResponse {
  restaurants: Restaurant[];
  total: number;
}