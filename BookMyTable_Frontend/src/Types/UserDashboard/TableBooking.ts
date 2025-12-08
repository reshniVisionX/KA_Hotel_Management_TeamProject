export interface TableBooking {
  id: string;
  restaurantName: string;
  tableNumber: string;
  bookingDate: string;
  bookingTime: string;
  numberOfGuests: number;
  status: 'pending' | 'confirmed' | 'cancelled' | 'completed';
  specialRequests?: string;
}

export interface TableBookingResponse {
  bookings: TableBooking[];
  total: number;
}