import { RoomResponse } from './room.types';

// Filter room
export interface RoomFilters {
    search: string;
    city: string;
    region: string;
    minPrice: number;
    maxPrice: number;
    activitiesIds: string[];
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
    pagination: RoomsPagination;
}