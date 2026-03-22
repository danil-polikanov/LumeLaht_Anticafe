import { configureStore } from '@reduxjs/toolkit';
import { roomReducer } from '@/entities/room/';

export const store = configureStore({
  reducer: {
    rooms: roomReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        // Ignore these action types
        ignoredActions: ['rooms/fetchRoomsByFilters/pending'],
      },
    }),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
