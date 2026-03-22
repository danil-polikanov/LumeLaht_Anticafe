// API client
export { RoomClient, SwaggerException } from './roomClient';

// Slice (reducer + actions)
export * from './roomSlice';

// Selectors
export * from './roomSelectors';

// Thunks (async operations)
export {
  fetchRooms,
  fetchRoomById,
  createRoom,
  updateRoom,
  deleteRoom,
  fetchActivities,
  fetchRoomsByFilters,
} from './roomThunks';
