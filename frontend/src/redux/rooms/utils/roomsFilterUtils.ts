// store/rooms/utils/roomsFiltersUtils.ts
import { RoomsState } from '../RoomsState';

export const applyFiltersAndSorting = (state: RoomsState) => {
    let filtered = [...state.rooms];

    if (state.filters.search) {
        filtered = filtered.filter(
            (room) =>
                room.name
                    ?.toLowerCase()
                    .includes(state.filters.search.toLowerCase()) ||
                room.description
                    ?.toLowerCase()
                    .includes(state.filters.search.toLowerCase())
        );
    }

    if (state.filters.city) {
        filtered = filtered.filter((room) =>
            room.address?.city
                ?.toLowerCase()
                .includes(state.filters.city.toLowerCase())
        );
    }

    if (state.filters.region) {
        filtered = filtered.filter((room) =>
            room.address?.region
                ?.toLowerCase()
                .includes(state.filters.region.toLowerCase())
        );
    }

    if (state.filters.minPrice > 0) {
        filtered = filtered.filter(
            (room) => (room.pricePerHour || 0) >= state.filters.minPrice
        );
    }

    if (state.filters.maxPrice < 10000) {
        filtered = filtered.filter(
            (room) => (room.pricePerHour || 0) <= state.filters.maxPrice
        );
    }

    if (state.filters.activities.length > 0) {
        filtered = filtered.filter((room) =>
            room.activity?.some(
                (activity) =>
                    activity.activityId !== undefined &&
                    state.filters.activities.includes(activity.activityId)
            )
        );
    }

    filtered = filtered.filter((room) => room.status === 'Active');

    filtered.sort((a, b) => {
        let aValue: any;
        let bValue: any;

        switch (state.sorting.field) {
            case 'name':
                aValue = a.name || '';
                bValue = b.name || '';
                break;
            case 'pricePerHour':
                aValue = a.pricePerHour || 0;
                bValue = b.pricePerHour || 0;
                break;
            case 'city':
                aValue = a.address?.city || '';
                bValue = b.address?.city || '';
                break;
            default:
                return 0;
        }

        return state.sorting.direction === 'asc'
            ? aValue < bValue
                ? -1
                : aValue > bValue
                ? 1
                : 0
            : aValue > bValue
            ? -1
            : aValue < bValue
            ? 1
            : 0;
    });

    state.pagination.total = filtered.length;
    state.pagination.totalPages = Math.ceil(
        filtered.length / state.pagination.limit
    );

    const startIndex = (state.pagination.page - 1) * state.pagination.limit;
    const endIndex = startIndex + state.pagination.limit;
    state.filteredRooms = filtered.slice(startIndex, endIndex);
};
