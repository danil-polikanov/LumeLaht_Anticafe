import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RoomFilters, RoomSorting, RoomsPagination } from '@/shared/types/filters.types';

export interface RoomsState {
  filters: RoomFilters;
  sorting: RoomSorting;
  pagination: RoomsPagination;
}

const initialState: RoomsState = {
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
    setPageSize: (state, action: PayloadAction<number>) => {
      state.pagination.pageSize = action.payload;
      state.pagination.currentPage = 1;
    },
    setPagination: (state, action: PayloadAction<RoomsPagination>) => {
      state.pagination = action.payload;
    },
  },
});

export const { setFilters, resetFilters, setSorting, setPage, setPageSize, setPagination } =
  roomSlice.actions;

export const roomReducer = roomSlice.reducer;
