import { RootState } from '@/app/providers/store';

// Room selectors
export const selectRooms = (state: RootState) => state.rooms.rooms;
export const selectSelectedRoom = (state: RootState) =>
    state.rooms.selectedRoom;
export const selectActivities = (state: RootState) => state.rooms.activities;
export const selectLoading = (state: RootState) => state.rooms.loading;
export const selectError = (state: RootState) => state.rooms.error;

// Filter selectors
export const selectFilters = (state: RootState) => state.rooms.filters;
export const selectSorting = (state: RootState) => state.rooms.sorting;
export const selectPagination = (state: RootState) => state.rooms.pagination;

// Computed selectors
export const selectHasRooms = (state: RootState) =>
    state.rooms.rooms.length > 0;
export const selectIsFirstPage = (state: RootState) =>
    state.rooms.pagination.currentPage === 1;
export const selectIsLastPage = (state: RootState) =>
    state.rooms.pagination.currentPage === state.rooms.pagination.totalPages;