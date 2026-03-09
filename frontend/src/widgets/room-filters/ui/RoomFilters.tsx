import React, { useState } from 'react';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
import { resetFilters, setFilters } from '@/entities/room/model';
import { selectFilters, selectActivities } from '@/entities/room/';
import styles from '@/pages/rooms/ui/RoomList.module.css';
import { useDebounce } from '@/shared/lib/hooks/useDebounce';

export const RoomFilters: React.FC = () => {
  const dispatch = useAppDispatch();
  const filters = useAppSelector(selectFilters);
  const activities = useAppSelector(selectActivities);
  const [isCollapsed, setIsCollapsed] = useState(false);

  // Debounce hook for all text fields
  const debouncedSetFilter = useDebounce((key: string, value: string | number) => {
    dispatch(setFilters({ [key]: value }));
  }, 500);

  // Local state for all text fields
  const [localSearch, setLocalSearch] = useState(filters.search);
  const [localCity, setLocalCity] = useState(filters.city);
  const [localRegion, setLocalRegion] = useState(filters.region);

  // Handlers for text fields with debounce
  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setLocalSearch(value); // update local state immediately
    debouncedSetFilter('search', value); // dispatch to Redux with delay
  };

  const handleCityChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setLocalCity(value);
    debouncedSetFilter('city', value);
  };

  const handleRegionChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setLocalRegion(value);
    debouncedSetFilter('region', value);
  };

  // Handlers for other fields (no debounce)
  // Numeric fields (price) — dispatch to Redux immediately, fewer changes expected
  const handleFilterChange = (filterKey: string, value: string | number) => {
    dispatch(setFilters({ [filterKey]: value }));
  };

  // Activities — checkboxes, dispatch to Redux immediately
  const handleActivityToggle = (activityId: string) => {
    const newActivities = filters.activitiesIds.includes(activityId)
      ? filters.activitiesIds.filter((id) => id !== activityId)
      : [...filters.activitiesIds, activityId];

    dispatch(setFilters({ activitiesIds: newActivities }));
  };

  // Clear filters
  const clearFilters = () => {
    dispatch(resetFilters());
  };

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
            <i className={`fas fa-chevron-${isCollapsed ? 'down' : 'up'}`}></i>
          </button>
        </div>
      </div>

      <div className={styles.collapse_filter_rooms}>
        <div className={`collapse ${!isCollapsed ? 'show' : ''}`}>
          <div className="card-body">
            <div className="row g-3">
              {/* Search with debounce */}
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
                  value={localSearch}
                  onChange={handleSearchChange}
                />
                {/* Debounce indicator */}
                {localSearch !== filters.search && (
                  <small className="text-muted d-block mt-1">
                    <i className="fas fa-hourglass-half me-1"></i>
                    Applying filter...
                  </small>
                )}
              </div>

              {/* City with debounce */}
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
                  value={localCity}
                  onChange={handleCityChange}
                />
                {localCity !== filters.city && (
                  <small className="text-muted d-block mt-1">
                    <i className="fas fa-hourglass-half me-1"></i>
                    Applying filter...
                  </small>
                )}
              </div>

              {/* Region with debounce */}
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
                  value={localRegion}
                  onChange={handleRegionChange}
                />
                {localRegion !== filters.region && (
                  <small className="text-muted d-block mt-1">
                    <i className="fas fa-hourglass-half me-1"></i>
                    Applying filter...
                  </small>
                )}
              </div>

              {/* Price (no debounce) */}
              <div className="col-md-3">
                <label htmlFor="minPrice" className="form-label">
                  <i className="fas fa-euro-sign me-1"></i>
                  Price from
                </label>
                <input
                  type="number"
                  id="minPrice"
                  className="form-control"
                  min="0"
                  placeholder="From"
                  value={filters.minPrice === 0 ? '' : filters.minPrice}
                  onChange={(e) =>
                    handleFilterChange(
                      'minPrice',
                      e.target.value === '' ? 0 : Number(e.target.value),
                    )
                  }
                />
              </div>

              <div className="col-md-3">
                <label htmlFor="maxPrice" className="form-label">
                  <i className="fas fa-euro-sign me-1"></i>
                  Price to
                </label>
                <input
                  type="number"
                  id="maxPrice"
                  className="form-control"
                  min="0"
                  placeholder="To"
                  value={
                    filters.maxPrice === 10000 || filters.maxPrice === 0 ? '' : filters.maxPrice
                  }
                  onChange={(e) =>
                    handleFilterChange(
                      'maxPrice',
                      e.target.value === '' ? 0 : Number(e.target.value),
                    )
                  }
                />
              </div>

              {/* Clear button */}
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

            {/* Activities */}
            <div className="mt-4">
              <label className="form-label">
                <i className="fas fa-running me-1"></i>
                List Activities
              </label>
              <div className="row">
                {activities.map((activity) => (
                  <div key={activity.activityId} className="col-md-3 col-sm-6">
                    <div className="form-check">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id={`activity-${activity.activityId}`}
                        checked={filters.activitiesIds.includes(activity.activityId ?? '')}
                        onChange={() => handleActivityToggle(activity.activityId ?? '')}
                      />
                      <label
                        className="form-check-label"
                        htmlFor={`activity-${activity.activityId}`}
                      >
                        {activity.name}
                      </label>
                    </div>
                  </div>
                ))}
              </div>
            </div>

            {/* Active filters indicator */}
            {(filters.search ||
              filters.city ||
              filters.region ||
              filters.minPrice > 0 ||
              filters.maxPrice < 10000 ||
              filters.activitiesIds.length > 0) && (
              <div className="mt-3">
                <div className="alert alert-info d-flex align-items-center">
                  <i className="fas fa-info-circle me-2"></i>
                  <span>
                    Active filters:{' '}
                    {filters.search && <span className="badge bg-primary me-1">Search</span>}
                    {filters.city && <span className="badge bg-primary me-1">City</span>}
                    {filters.region && <span className="badge bg-primary me-1">Region</span>}
                    {filters.minPrice > 0 && (
                      <span className="badge bg-primary me-1">Min. price</span>
                    )}
                    {filters.maxPrice < 10000 && (
                      <span className="badge bg-primary me-1">Max. Price</span>
                    )}
                    {filters.activitiesIds.length > 0 && (
                      <span className="badge bg-primary me-1">
                        Activities ({filters.activitiesIds.length})
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
