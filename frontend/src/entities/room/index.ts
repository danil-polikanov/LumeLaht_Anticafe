// index.ts — единая точка входа для модели "rooms"

// API‑клиент
export { RoomClient, SwaggerException } from './model/roomClient';

// Slice (редьюсер + экшены)
export * from './model/roomSlice';

// Селекторы
export * from './model/roomSelectors';

// Thunks (асинхронные операции)
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
