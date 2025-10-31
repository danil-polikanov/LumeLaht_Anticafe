import { useCallback, useRef } from 'react';
import { useAppDispatch } from '../redux/hooks';
import { setFilters } from '../redux/rooms/roomSlice';

export function useDebouncedFilter(delay: number = 500) {
    const dispatch = useAppDispatch();
    const timeoutRef = useRef<NodeJS.Timeout | null>(null);

    const debouncedSetFilter = useCallback(
        (filterKey: string, value: any) => {
            // Очищаем предыдущий таймаут
            if (timeoutRef.current) {
                clearTimeout(timeoutRef.current);
            }

            // Устанавливаем новый таймаут
            timeoutRef.current = setTimeout(() => {
                dispatch(setFilters({ [filterKey]: value }));
            }, delay);
        },
        [dispatch, delay]
    );

    return debouncedSetFilter;
}
