import { baseApi } from '@/shared/api/baseApi';
import { RoomResponse, CreateRoomRequest, ActivityResponse } from '@/shared/types/room.types';
import { PagedRoomsResponse, RoomFilters, RoomSorting } from '@/shared/types/filters.types';

interface RoomsByFiltersArgs {
  filters: RoomFilters;
  sorting: RoomSorting;
  currentPage: number;
  pageSize: number;
}

export const roomApi = baseApi.injectEndpoints({
  endpoints: (builder) => ({
    getRoomsByFilters: builder.query<PagedRoomsResponse, RoomsByFiltersArgs>({
      query: ({ filters, sorting, currentPage, pageSize }) => ({
        url: '/room/filters',
        method: 'POST',
        body: {
          roomOptionDTO: filters,
          sortOptions: sorting,
          page: currentPage,
          pageSize,
        },
      }),
      providesTags: (result) =>
        result
          ? [
              ...result.items.map((room) => ({ type: 'Room' as const, id: room.roomId })),
              { type: 'Room', id: 'LIST' },
            ]
          : [{ type: 'Room', id: 'LIST' }],
    }),

    getRoomById: builder.query<RoomResponse, string>({
      query: (id) => `/room/${id}`,
      providesTags: (_result, _error, id) => [{ type: 'Room', id }],
    }),

    getActivities: builder.query<ActivityResponse[], void>({
      query: () => '/room/activities',
      providesTags: ['Activity'],
    }),

    createRoom: builder.mutation<RoomResponse, CreateRoomRequest>({
      query: (room) => ({
        url: '/room',
        method: 'POST',
        body: room,
      }),
      invalidatesTags: [{ type: 'Room', id: 'LIST' }],
    }),

    updateRoom: builder.mutation<RoomResponse, { id: string; data: CreateRoomRequest }>({
      query: ({ id, data }) => ({
        url: `/room/${id}`,
        method: 'PUT',
        body: data,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: 'Room', id },
        { type: 'Room', id: 'LIST' },
      ],
    }),

    deleteRoom: builder.mutation<void, string>({
      query: (id) => ({
        url: `/room/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: (_result, _error, id) => [
        { type: 'Room', id },
        { type: 'Room', id: 'LIST' },
      ],
    }),
  }),
});

export const {
  useGetRoomsByFiltersQuery,
  useGetRoomByIdQuery,
  useGetActivitiesQuery,
  useCreateRoomMutation,
  useUpdateRoomMutation,
  useDeleteRoomMutation,
} = roomApi;
