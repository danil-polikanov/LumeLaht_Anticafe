import { ActivityResponse } from '../roomTypes/ActivityResponse';
import { RoomResponse } from '../roomTypes/RoomResponse';

// Filter room
export interface RoomFilters {
    search: string;
    city: string;
    region: string;
    minPrice: number;
    maxPrice: number;
    activities: string[];
    status: string;
}

export interface RoomsPagination {
    currentPage: number;
    pageSize: number;
    totalItems: number;
    totalPages: number;
}
export interface RoomSorting {
    field: 'name' | 'pricePerHour' | 'city';
    direction: 'asc' | 'desc';
}
export interface PagedRoomsResponse {
    items: RoomResponse[];
    totalItems: number;
    currentPage: number;
    pageSize: number;
    totalPages: number;
}
