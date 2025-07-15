import { RoomResponse } from '../../types/roomTypes/RoomResponse';
import {
    RoomFilters,
    RoomSorting,
    RoomsPagination,
} from '../../types/roomShowTypes/RoomFilters';

export interface RoomsState {
    rooms: RoomResponse[];
    filteredRooms: RoomResponse[];
    selectedRoom: RoomResponse | null;
    loading: boolean;
    error: string | null;
    filters: RoomFilters;
    sorting: RoomSorting;
    pagination: RoomsPagination;
}
