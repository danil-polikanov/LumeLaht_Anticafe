// index.ts — single entry point for the "rooms" model

// API client
export { RoomClient, SwaggerException } from './model/roomClient';

// Slice (reducer + actions)
export * from './model/roomSlice';

// Selectors
export * from './model/roomSelectors';

// Thunks (async operations)
export {
  fetchRooms,
  fetchRoomById,
  createRoom,
  updateRoom,
  deleteRoom,
  fetchActivities,
  fetchRoomsByFilters,
} from './model/roomThunks';
export { RoomApi } from './api/roomApi';
