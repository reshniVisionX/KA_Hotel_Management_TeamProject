export interface AddReviewRequest {
  restaurantId: number;
  userId: number;
  rating: number;
  comments: string;
}

export interface AddReviewResponse {
  success: boolean;
  message: string;
  data: boolean;
}
