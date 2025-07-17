// store/rooms/roomsSlice.ts
import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import {
    RoomFilters,
    RoomSorting,
} from '../../types/roomShowTypes/RoomFilters';
import { RoomsState } from './RoomsState';
import { fetchRooms, fetchRoomById } from '../../api/roomsThunks';
import { applyFiltersAndSorting } from './utils/roomsFilterUtils';

const initialState: RoomsState = {
    rooms: [],
    filteredRooms: [],
    selectedRoom: null,
    loading: false,
    error: null,
    filters: {
        search: '',
        city: '',
        region: '',
        minPrice: 0,
        maxPrice: 10000,
        activities: [],
        isActive: true,
    },
    sorting: {
        field: 'name',
        direction: 'asc',
    },
    pagination: {
        page: 1,
        limit: 3,
        total: 0,
        totalPages: 0,
    },
};

const roomsSlice = createSlice({
    name: 'rooms',
    initialState,
    reducers: {
        setFilters: (state, action: PayloadAction<Partial<RoomFilters>>) => {
            state.filters = { ...state.filters, ...action.payload };
            state.pagination.page = 1;
            applyFiltersAndSorting(state);
        },
        setSorting: (state, action: PayloadAction<RoomSorting>) => {
            state.sorting = action.payload;
            applyFiltersAndSorting(state);
        },
        setPage: (state, action: PayloadAction<number>) => {
            state.pagination.page = action.payload;
            applyFiltersAndSorting(state);
        },
        setLimit: (state, action: PayloadAction<number>) => {
            state.pagination.limit = action.payload;
            state.pagination.page = 1;
            applyFiltersAndSorting(state);
        },
        setSelectedRoom: (
            state,
            action: PayloadAction<RoomsState['selectedRoom']>
        ) => {
            state.selectedRoom = action.payload;
        },
        clearSelectedRoom: (state) => {
            state.selectedRoom = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchRooms.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchRooms.fulfilled, (state, action) => {
                state.loading = false;
                state.rooms = action.payload;
                applyFiltersAndSorting(state);
            })
            .addCase(fetchRooms.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload ?? 'Ошибка';
            })
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
                state.error = action.payload ?? 'Ошибка';
            });
    },
});

export const {
    setFilters,
    setSorting,
    setPage,
    setLimit,
    setSelectedRoom,
    clearSelectedRoom,
} = roomsSlice.actions;
export default roomsSlice.reducer;
