export interface CreateBookingRequest {
  roomId: string;
  startTime: string;
}

export interface BookingResponse {
  bookingId: string;
  roomId: string;
  roomName: string;
  startTime: string;
  endTime: string;
  totalPrice: number;
  status: string;
  createdAt: string;
}
