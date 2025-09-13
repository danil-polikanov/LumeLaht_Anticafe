// Filter room
export interface RoomFilters {
    search: string;
    city: string;
    region: string;
    minPrice: number;
    maxPrice: number;
    activities: string[];
    status: boolean;
}

export interface RoomsPagination {
    page: number;
    limit: number;
    total: number;
    totalPages: number;
}
export interface RoomSorting {
    field: 'name' | 'pricePerHour' | 'city';
    direction: 'asc' | 'desc';
}
