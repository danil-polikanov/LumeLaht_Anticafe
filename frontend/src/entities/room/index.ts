// Slice (reducer + actions)
export * from './model/roomSlice';

// Selectors
export * from './model/roomSelectors';

// RTK Query API
export {
  roomApi,
  useGetRoomsByFiltersQuery,
  useGetRoomByIdQuery,
  useGetActivitiesQuery,
  useCreateRoomMutation,
  useUpdateRoomMutation,
  useDeleteRoomMutation,
} from './api/roomApi';
