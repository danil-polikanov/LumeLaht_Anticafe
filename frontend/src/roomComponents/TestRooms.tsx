import React, { useEffect, useState } from 'react';
import '../roomComponents/TestRooms.css';
import { useAppDispatch, useAppSelector } from '../redux/hooks'; // создайте typed hooks
import {
    setFilters,
    resetFilters,
    setSorting,
    setPage,
    setLimit,
    setSelectedRoom,
    clearSelectedRoom,
    clearError,
} from '../redux/rooms/roomSlice';
import { fetchRoomsByFilters, fetchActivities } from '../api/roomsThunks';
import {
    selectRooms,
    selectActivities,
    selectLoading,
    selectError,
    selectFilters,
    selectSorting,
    selectPagination,
} from '../redux/rooms/roomsSelectors';
const TestRooms: React.FC = () => {
    const dispatch = useAppDispatch();
    // Селекторы
    const rooms = useAppSelector(selectRooms);
    const activities = useAppSelector(selectActivities);
    const loading = useAppSelector(selectLoading);
    const error = useAppSelector(selectError);
    const filters = useAppSelector(selectFilters);
    const sorting = useAppSelector(selectSorting);
    const pagination = useAppSelector(selectPagination);

    // Локальное состояние для контролируемых инпутов
    const [localSearch, setLocalSearch] = useState(filters.search);
    const [localMinPrice, setLocalMinPrice] = useState(filters.minPrice);
    const [localMaxPrice, setLocalMaxPrice] = useState(filters.maxPrice);

    // Загрузка активностей при монтировании
    useEffect(() => {
        dispatch(fetchActivities());
    }, [dispatch]);

    // Загрузка комнат при изменении фильтров, сортировки или страницы
    useEffect(() => {
        dispatch(fetchRoomsByFilters());
    }, [
        dispatch,
        filters,
        sorting,
        pagination.currentPage,
        pagination.pageSize,
    ]);

    // Обработчик применения фильтров
    const handleApplyFilters = () => {
        dispatch(
            setFilters({
                search: localSearch,
                minPrice: localMinPrice,
                maxPrice: localMaxPrice,
            })
        );
    };

    // Обработчик сброса фильтров
    const handleResetFilters = () => {
        setLocalSearch('');
        setLocalMinPrice(0);
        setLocalMaxPrice(10000);
        dispatch(resetFilters());
    };

    // Обработчик изменения статуса
    const handleStatusChange = (status: string) => {
        dispatch(setFilters({ status }));
    };

    // Обработчик изменения активностей
    const handleActivityToggle = (activityId: string) => {
        const currentActivities = [...filters.activities];
        const index = currentActivities.indexOf(activityId);

        if (index > -1) {
            currentActivities.splice(index, 1);
        } else {
            currentActivities.push(activityId);
        }

        dispatch(setFilters({ activities: currentActivities }));
    };

    // Обработчик изменения сортировки
    const handleSortChange = (field: 'name' | 'pricePerHour' | 'city') => {
        const newOrder =
            sorting.field === field && sorting.direction === 'asc'
                ? 'desc'
                : 'asc';

        dispatch(setSorting({ field, direction: newOrder }));
    };

    // Обработчик изменения страницы
    const handlePageChange = (newPage: number) => {
        dispatch(setPage(newPage));
    };

    // Генерация массива страниц для пагинации
    const pageNumbers = Array.from(
        { length: pagination.totalPages },
        (_, i) => i + 1
    );

    if (error) {
        return <div className="error">Ошибка: {error}</div>;
    }

    return (
        <div className="rooms-container">
            {/* ФИЛЬТРЫ */}
            <div className="filters-section">
                <h2>Фильтры</h2>

                {/* Поиск */}
                <div className="filter-group">
                    <label>Поиск</label>
                    <input
                        type="text"
                        value={localSearch}
                        onChange={(e) => setLocalSearch(e.target.value)}
                        placeholder="Название или описание..."
                    />
                </div>

                {/* Статус */}
                <div className="filter-group">
                    <label>Статус</label>
                    <select
                        value={filters.status}
                        onChange={(e) => handleStatusChange(e.target.value)}
                    >
                        <option value="">Все</option>
                        <option value="available">Доступно</option>
                        <option value="occupied">Занято</option>
                        <option value="maintenance">На обслуживании</option>
                    </select>
                </div>

                {/* Цена */}
                <div className="filter-group">
                    <label>Цена (₽/час)</label>
                    <div className="price-inputs">
                        <input
                            type="number"
                            value={localMinPrice}
                            onChange={(e) =>
                                setLocalMinPrice(Number(e.target.value))
                            }
                            placeholder="От"
                            min="0"
                        />
                        <span>-</span>
                        <input
                            type="number"
                            value={localMaxPrice}
                            onChange={(e) =>
                                setLocalMaxPrice(Number(e.target.value))
                            }
                            placeholder="До"
                            min="0"
                        />
                    </div>
                </div>

                {/* Активности */}
                <div className="filter-group">
                    <label>Активности</label>
                    <div className="activities-checkboxes">
                        {activities.map((activity) => (
                            <label key={activity.activityId}>
                                <input
                                    type="checkbox"
                                    checked={
                                        activity.activityId !== undefined &&
                                        filters.activities.includes(
                                            String(activity.activityId)
                                        )
                                    }
                                    onChange={() =>
                                        activity.activityId !== undefined &&
                                        handleActivityToggle(
                                            String(activity.activityId)
                                        )
                                    }
                                />
                                {activity.name}
                            </label>
                        ))}
                    </div>
                </div>

                {/* Кнопки */}
                <div className="filter-buttons">
                    <button onClick={handleApplyFilters}>Применить</button>
                    <button onClick={handleResetFilters}>Сбросить</button>
                </div>
            </div>

            {/* СПИСОК КОМНАТ */}
            <div className="rooms-section">
                {/* Сортировка */}
                <div className="sorting-controls">
                    <button onClick={() => handleSortChange('name')}>
                        По названию
                        {sorting.field === 'name' &&
                            (sorting.direction === 'asc' ? ' ↑' : ' ↓')}
                    </button>
                    <button onClick={() => handleSortChange('pricePerHour')}>
                        По цене
                        {sorting.field === 'pricePerHour' &&
                            (sorting.direction === 'asc' ? ' ↑' : ' ↓')}
                    </button>
                </div>

                {/* Результаты */}
                <div className="rooms-info">
                    Найдено: {pagination.totalItems} комнат(ы)
                </div>

                {/* Загрузка */}
                {loading && <div className="loading">Загрузка...</div>}

                {/* Список комнат */}
                {!loading && (
                    <div className="rooms-grid">
                        {rooms.length === 0 ? (
                            <div className="no-results">Комнаты не найдены</div>
                        ) : (
                            rooms.map((room) => (
                                <div key={room.roomId} className="room-card">
                                    <h3>{room.name}</h3>
                                    <p>{room.description}</p>
                                    <p className="price">
                                        {room.pricePerHour} ₽/час
                                    </p>
                                    <span className={`status ${room.status}`}>
                                        {room.status}
                                    </span>
                                </div>
                            ))
                        )}
                    </div>
                )}

                {/* ПАГИНАЦИЯ */}
                {!loading && pagination.totalPages > 1 && (
                    <div className="pagination">
                        <button
                            onClick={() =>
                                handlePageChange(pagination.currentPage - 1)
                            }
                            disabled={pagination.currentPage === 1}
                        >
                            Назад
                        </button>

                        {pageNumbers.map((pageNum) => (
                            <button
                                key={pageNum}
                                onClick={() => handlePageChange(pageNum)}
                                className={
                                    pagination.currentPage === pageNum
                                        ? 'active'
                                        : ''
                                }
                            >
                                {pageNum}
                            </button>
                        ))}

                        <button
                            onClick={() =>
                                handlePageChange(pagination.currentPage + 1)
                            }
                            disabled={
                                pagination.currentPage === pagination.totalPages
                            }
                        >
                            Вперед
                        </button>

                        {/* Изменение размера страницы */}
                        <select
                            value={pagination.pageSize}
                            onChange={(e) =>
                                dispatch(setPage(Number(e.target.value)))
                            }
                        >
                            <option value={5}>5 на странице</option>
                            <option value={10}>10 на странице</option>
                            <option value={20}>20 на странице</option>
                            <option value={50}>50 на странице</option>
                        </select>
                    </div>
                )}
            </div>
        </div>
    );
};

export default TestRooms;
