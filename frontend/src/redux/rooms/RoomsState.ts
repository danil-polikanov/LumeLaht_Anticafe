import { RoomResponse } from '../../types/roomTypes/RoomResponse';
import {
    RoomFilters,
    RoomSorting,
    RoomsPagination,
} from '../../types/roomShowTypes/RoomFilters';
import { ActivityResponse } from '../../types/roomTypes/ActivityResponse';

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
