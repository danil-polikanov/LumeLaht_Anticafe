import { baseApi } from '@/shared/api/baseApi';
import { CreateBookingRequest, BookingResponse } from '@/shared/types';

export const bookingApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    createBooking: builder.mutation<BookingResponse, CreateBookingRequest>({
      query: (data) => ({
        url: '/booking',
        method: 'POST',
        body: data,
      }),
      invalidatesTags: [{ type: 'Booking', id: 'LIST' }],
    }),

    getMyBookings: builder.query<BookingResponse[], void>({
      query: () => '/booking/my',
      providesTags: (result) =>
        result
          ? [
              ...result.map((b) => ({ type: 'Booking' as const, id: b.bookingId })),
              { type: 'Booking', id: 'LIST' },
            ]
          : [{ type: 'Booking', id: 'LIST' }],
    }),

    cancelBooking: builder.mutation<void, string>({
      query: (bookingId) => ({
        url: `/booking/${bookingId}`,
        method: 'DELETE',
      }),
      invalidatesTags: (_result, _error, bookingId) => [
        { type: 'Booking', id: bookingId },
        { type: 'Booking', id: 'LIST' },
      ],
    }),

    getRoomBookings: builder.query<BookingResponse[], { roomId: string; date: string }>({
      query: ({ roomId, date }) => `/booking/room/${roomId}?date=${date}`,
      providesTags: [{ type: 'Booking', id: 'ROOM' }],
    }),
  }),
});

export const {
  useCreateBookingMutation,
  useGetMyBookingsQuery,
  useCancelBookingMutation,
  useGetRoomBookingsQuery,
} = bookingApi;
