// store/rooms/roomsSlice.ts
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {
    RoomFilters,
    RoomSorting,
} from '../../types/roomShowTypes/RoomFilters';
import { RoomsState } from './RoomsState';
import {
    fetchRooms,
    fetchRoomById,
    fetchRoomsByFilters,
    fetchActivities,
} from '../../api/roomsThunks';
import { applyFiltersAndSorting } from './utils/roomsFilterUtils';
import { RoomResponse } from '../../types/roomTypes/RoomResponse';

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

const roomsSlice = createSlice({
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
            // 1️⃣ Вывести все комнаты
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
            // 2️⃣ Вывести комнату по ID
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
        //3️⃣ Вывести активности
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
} = roomsSlice.actions;
export default roomsSlice.reducer;
