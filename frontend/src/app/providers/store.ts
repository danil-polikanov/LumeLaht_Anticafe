import { configureStore } from '@reduxjs/toolkit';
import { roomReducer } from '@/entities/room/';
import { authReducer } from '@/entities/auth';
import { baseApi } from '@/shared/api/baseApi';

export const store = configureStore({
  reducer: {
    rooms: roomReducer,
    auth: authReducer,
    [baseApi.reducerPath]: baseApi.reducer,
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(baseApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
