// store/rooms/roomsSelectors.ts
import { RoomsState } from './RoomsState';

export const selectRooms = (state: { rooms: RoomsState }) => state.rooms.rooms;
export const selectSelectedRoom = (state: { rooms: RoomsState }) =>
    state.rooms.selectedRoom;
export const selectActivities = (state: { rooms: RoomsState }) =>
    state.rooms.activities;
export const selectLoading = (state: { rooms: RoomsState }) =>
    state.rooms.loading;
export const selectError = (state: { rooms: RoomsState }) => state.rooms.error;
export const selectFilters = (state: { rooms: RoomsState }) =>
    state.rooms.filters;
export const selectSorting = (state: { rooms: RoomsState }) =>
    state.rooms.sorting;
export const selectPagination = (state: { rooms: RoomsState }) =>
    state.rooms.pagination;
