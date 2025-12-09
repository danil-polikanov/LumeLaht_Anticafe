// API‑клиент
export { RoomClient, SwaggerException } from './roomClient';

// Slice (редьюсер + экшены)
export * from './roomSlice';

// Селекторы
export * from './roomSelectors';

// Thunks (асинхронные операции)
export {
    fetchRooms,
    fetchRoomById,
    createRoom,
    updateRoom,
    deleteRoom,
    fetchActivities,
    fetchRoomsByFilters,
} from './roomThunks';
