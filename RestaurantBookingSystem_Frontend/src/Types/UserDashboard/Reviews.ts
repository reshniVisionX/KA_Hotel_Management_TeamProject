export interface Review {
  id: string;
  restaurantName: string;
  rating: number;
  comment: string;
  reviewDate: string;
  orderId?: string;
}

export interface ReviewsResponse {
  reviews: Review[];
  total: number;
}