export interface Reservation {
  Id: number;
  CustomerId: number;
  RestaurantId: number;
  ReservationDate: string;
  ReservationTime: string;
  PartySize: number;
  Status: string;
  SpecialRequests?: string;
  CreatedAt: string;
  UpdatedAt: string;
}

export interface ReservationSummary {
  totalReservations: number;
  todayReservations: number;
  upcomingReservations: number;
  completedReservations: number;
  cancelledReservations: number;
}