// store/rooms/roomsSlice.ts
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RoomFilters, RoomSorting, RoomsPagination } from '@/shared/types/filters.types';
import { fetchRoomById, fetchRoomsByFilters, fetchActivities } from '@/entities/room';
import { RoomResponse, ActivityResponse } from '@/shared/types/room.types';
export interface RoomsState {
  rooms: RoomResponse[];
  selectedRoom: RoomResponse | null;
  activities: ActivityResponse[];
  loading: boolean;
  error: string | null;
  filters: RoomFilters;
  sorting: RoomSorting;
  pagination: RoomsPagination;
}

const initialState: RoomsState = {
  rooms: [],
  selectedRoom: null,
  activities: [],
  loading: false,
  error: null,
  filters: {
    search: '',
    city: '',
    region: '',
    minPrice: 0,
    maxPrice: 10000,
    activitiesIds: [],
    status: '',
  },
  sorting: {
    field: 'name',
    direction: 'asc',
  },
  pagination: {
    currentPage: 1,
    pageSize: 3,
    totalItems: 0,
    totalPages: 0,
  },
};

export const roomSlice = createSlice({
  name: 'rooms',
  initialState,
  reducers: {
    setFilters: (state, action: PayloadAction<Partial<RoomFilters>>) => {
      state.filters = { ...state.filters, ...action.payload };
      state.pagination.currentPage = 1;
    },
    resetFilters: (state) => {
      state.filters = initialState.filters;
      state.pagination.currentPage = 1;
    },
    setSorting: (state, action: PayloadAction<RoomSorting>) => {
      state.sorting = action.payload;
    },
    setPage: (state, action: PayloadAction<number>) => {
      state.pagination.currentPage = action.payload;
    },
    setLimit: (state, action: PayloadAction<number>) => {
      state.pagination.totalPages = action.payload;
      state.pagination.currentPage = 1;
    },
    setSelectedRoom: (state, action: PayloadAction<RoomResponse>) => {
      state.selectedRoom = action.payload;
    },
    clearSelectedRoom: (state) => {
      state.selectedRoom = null;
    },
    clearError: (state) => {
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // 1️⃣ Fetch all rooms
      .addCase(fetchRoomsByFilters.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRoomsByFilters.fulfilled, (state, action) => {
        state.loading = false;
        state.rooms = action.payload.items;
        state.pagination = action.payload.pagination;
      })
      .addCase(fetchRoomsByFilters.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ?? 'Error';
      })
      // 2️⃣ Fetch room by ID
      .addCase(fetchRoomById.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchRoomById.fulfilled, (state, action) => {
        state.loading = false;
        state.selectedRoom = action.payload;
      })
      .addCase(fetchRoomById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ?? 'Error';
      });
    // 3️⃣ Fetch activities
    builder
      .addCase(fetchActivities.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchActivities.fulfilled, (state, action) => {
        state.loading = false;
        state.activities = action.payload;
      })
      .addCase(fetchActivities.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload ?? 'Error';
      });
  },
});

export const {
  setFilters,
  resetFilters,
  setSorting,
  setPage,
  setLimit,
  setSelectedRoom,
  clearSelectedRoom,
  clearError,
} = roomSlice.actions;

export const roomReducer = roomSlice.reducer;
