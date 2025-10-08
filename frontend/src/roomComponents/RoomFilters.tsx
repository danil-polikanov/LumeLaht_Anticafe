import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setFilters } from '../redux/rooms/roomSlice';
import { selectFilters } from '../redux/rooms/roomsSelectors';
import styles from './RoomList.module.css';
const RoomFilters: React.FC = () => {
    const dispatch = useDispatch();
    const filters = useSelector(selectFilters);
    const [isCollapsed, setIsCollapsed] = useState(false);

    const handleFilterChange = (filterKey: string, value: any) => {
        dispatch(setFilters({ [filterKey]: value }));
    };

    const handleActivityToggle = (activityId: string) => {
        const newActivities = filters.activities.includes(activityId)
            ? filters.activities.filter((name) => name !== activityId)
            : [...filters.activities, activityId];

        dispatch(setFilters({ activities: newActivities }));
    };

    const clearFilters = () => {
        dispatch(
            setFilters({
                search: '',
                city: '',
                region: '',
                minPrice: 0,
                maxPrice: 10000,
                activities: [],
                status: '',
            })
        );
    };

    // Примерные активности для фильтрации
    const availableActivities = [
        { id: 1, name: 'Monopolia' },
        { id: 2, name: 'Uno' },
        { id: 3, name: 'Billiards' },
        { id: 4, name: 'Bowling' },
        { id: 5, name: 'Scrabble' },
    ];

    return (
        <div className={`card mb-4 shadow-sm`}>
            <div className="card-header bg-primary bg-gradient text-white">
                <div className="d-flex justify-content-between align-items-center">
                    <h5 className="mb-0">
                        <i className="fas fa-filter me-2"></i>
                        Filters
                    </h5>
                    <button
                        className="btn btn-sm btn-outline-light"
                        onClick={() => setIsCollapsed(!isCollapsed)}
                    >
                        <i
                            className={`fas fa-chevron-${
                                isCollapsed ? 'down' : 'up'
                            }`}
                        ></i>
                    </button>
                </div>
            </div>

            <div className={styles.collapse_filter_rooms}>
                <div className={`collapse  ${!isCollapsed ? 'show' : ''}`}>
                    <div className="card-body">
                        <div className="row g-3">
                            {/* Поиск */}
                            <div className="col-md-6">
                                <label htmlFor="search" className="form-label">
                                    <i className="fas fa-search me-1"></i>
                                    Search
                                </label>
                                <input
                                    type="text"
                                    id="search"
                                    className="form-control"
                                    placeholder="Search by name or description..."
                                    value={filters.search}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            'search',
                                            e.target.value
                                        )
                                    }
                                />
                            </div>

                            {/* Город */}
                            <div className="col-md-3">
                                <label htmlFor="city" className="form-label">
                                    <i className="fas fa-map-marker-alt me-1"></i>
                                    City
                                </label>
                                <input
                                    type="text"
                                    id="city"
                                    className="form-control"
                                    placeholder="City"
                                    value={filters.city}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            'city',
                                            e.target.value
                                        )
                                    }
                                />
                            </div>

                            {/* Регион */}
                            <div className="col-md-3">
                                <label htmlFor="region" className="form-label">
                                    <i className="fas fa-globe me-1"></i>
                                    Region
                                </label>
                                <input
                                    type="text"
                                    id="region"
                                    className="form-control"
                                    placeholder="Region"
                                    value={filters.region}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            'region',
                                            e.target.value
                                        )
                                    }
                                />
                            </div>

                            {/* Цена от */}
                            <div className="col-md-3">
                                <label
                                    htmlFor="minPrice"
                                    className="form-label"
                                >
                                    <i className="fas fa-euro-sign me-1"></i>
                                    Price from
                                </label>
                                <input
                                    type="number"
                                    id="minPrice"
                                    className="form-control"
                                    min="0"
                                    placeholder="0"
                                    value={filters.minPrice}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            'minPrice',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>

                            {/* Цена до */}
                            <div className="col-md-3">
                                <label
                                    htmlFor="maxPrice"
                                    className="form-label"
                                >
                                    <i className="fas fa-euro-sign me-1"></i>
                                    Price to
                                </label>
                                <input
                                    type="number"
                                    id="maxPrice"
                                    className="form-control"
                                    min="0"
                                    placeholder="10000"
                                    value={filters.maxPrice}
                                    onChange={(e) =>
                                        handleFilterChange(
                                            'maxPrice',
                                            Number(e.target.value)
                                        )
                                    }
                                />
                            </div>

                            {/* Статус активности                  <div className="col-md-3">
                                <label className="form-label">
                                    <i className="fas fa-toggle-on me-1"></i>
                                    Status
                                </label>
                                <div className="form-check form-switch form-switch d-flex align-items-center justify-content-evenly">
                                    <input
                                        className="form-check-input"
                                        type="checkbox"
                                        id="isActive"
                                        role="switch"
                                        checked={filters.status}
                                        onChange={(e) =>
                                            handleFilterChange(
                                                'isActive',
                                                e.target.checked
                                            )
                                        }
                                    />
                                    <label
                                        className="form-check-label"
                                        htmlFor="isActive"
                                    >
                                        Active only
                                    </label>
                                </div>
                            </div>*/}

                            {/* Кнопка очистки */}
                            <div className="col-md-3 d-flex align-items-end">
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary w-100"
                                    onClick={clearFilters}
                                >
                                    <i className="fas fa-times me-1"></i>
                                    Clear
                                </button>
                            </div>
                        </div>

                        {/* Активности */}
                        <div className="mt-4">
                            <label className="form-label">
                                <i className="fas fa-running me-1"></i>
                                List Activities
                            </label>
                            <div className="row">
                                {availableActivities.map((activity) => (
                                    <div
                                        key={activity.id}
                                        className="col-md-3 col-sm-6"
                                    >
                                        <div className="form-check">
                                            <input
                                                className="form-check-input"
                                                type="checkbox"
                                                id={`activity-${activity.id}`}
                                                checked={filters.activities.includes(
                                                    activity.name
                                                )}
                                                onChange={() =>
                                                    handleActivityToggle(
                                                        activity.name
                                                    )
                                                }
                                            />
                                            <label
                                                className="form-check-label"
                                                htmlFor={`activity-${activity.id}`}
                                            >
                                                {activity.name}
                                            </label>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>

                        {/* Индикатор активных фильтров */}
                        {(filters.search ||
                            filters.city ||
                            filters.region ||
                            filters.minPrice > 0 ||
                            filters.maxPrice < 10000 ||
                            filters.activities.length > 0) && (
                            <div className="mt-3">
                                <div className="alert alert-info d-flex align-items-center">
                                    <i className="fas fa-info-circle me-2"></i>
                                    <span>
                                        Current filters:{' '}
                                        {filters.search && (
                                            <span className="badge bg-primary me-1">
                                                Search
                                            </span>
                                        )}
                                        {filters.city && (
                                            <span className="badge bg-primary me-1">
                                                City
                                            </span>
                                        )}
                                        {filters.region && (
                                            <span className="badge bg-primary me-1">
                                                Region
                                            </span>
                                        )}
                                        {filters.minPrice > 0 && (
                                            <span className="badge bg-primary me-1">
                                                Min. price
                                            </span>
                                        )}
                                        {filters.maxPrice < 10000 && (
                                            <span className="badge bg-primary me-1">
                                                Max. Price
                                            </span>
                                        )}
                                        {filters.activities.length > 0 && (
                                            <span className="badge bg-primary me-1">
                                                Activities (
                                                {filters.activities.length})
                                            </span>
                                        )}
                                    </span>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RoomFilters;
