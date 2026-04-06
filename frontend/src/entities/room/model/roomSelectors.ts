import { RootState } from '@/app/providers/store';

// Filter selectors
export const selectFilters = (state: RootState) => state.rooms.filters;
export const selectSorting = (state: RootState) => state.rooms.sorting;
export const selectPagination = (state: RootState) => state.rooms.pagination;

// Computed selectors
export const selectIsFirstPage = (state: RootState) => state.rooms.pagination.currentPage === 1;
export const selectIsLastPage = (state: RootState) =>
  state.rooms.pagination.currentPage === state.rooms.pagination.totalPages;
