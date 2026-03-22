import React, { useState } from 'react';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
import { resetFilters, setFilters } from '@/entities/room/model';
import { selectFilters, selectActivities } from '@/entities/room/';
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
    setLocalSearch(value);
    debouncedSetFilter('search', value);
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
  const handleFilterChange = (filterKey: string, value: string | number) => {
    dispatch(setFilters({ [filterKey]: value }));
  };

  // Activities — checkboxes
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
    <div className="bg-white rounded-lg shadow-sm mb-4">
      {/* Card header */}
      <div className="bg-gradient-to-r from-blue-600 to-blue-700 text-white p-4 rounded-t-lg">
        <div className="flex justify-between items-center">
          <h5 className="mb-0 font-semibold">
            <i className="fas fa-filter mr-2"></i>
            Filters
          </h5>
          <button
            className="border border-white text-white px-2 py-1 rounded text-sm hover:bg-white/10 bg-transparent cursor-pointer"
            onClick={() => setIsCollapsed(!isCollapsed)}
          >
            <i className={`fas fa-chevron-${isCollapsed ? 'down' : 'up'}`}></i>
          </button>
        </div>
      </div>

      <div className={`filter-collapse ${!isCollapsed ? 'open' : ''}`}>
        <div className="p-4">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
            {/* Search with debounce */}
            <div className="sm:col-span-2">
              <label htmlFor="search" className="block text-sm font-medium text-gray-700 mb-1">
                <i className="fas fa-search mr-1"></i>
                Search
              </label>
              <input
                type="text"
                id="search"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Search by name or description..."
                value={localSearch}
                onChange={handleSearchChange}
              />
              {localSearch !== filters.search && (
                <small className="text-gray-500 block mt-1">
                  <i className="fas fa-hourglass-half mr-1"></i>
                  Applying filter...
                </small>
              )}
            </div>

            {/* City with debounce */}
            <div>
              <label htmlFor="city" className="block text-sm font-medium text-gray-700 mb-1">
                <i className="fas fa-map-marker-alt mr-1"></i>
                City
              </label>
              <input
                type="text"
                id="city"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="City"
                value={localCity}
                onChange={handleCityChange}
              />
              {localCity !== filters.city && (
                <small className="text-gray-500 block mt-1">
                  <i className="fas fa-hourglass-half mr-1"></i>
                  Applying filter...
                </small>
              )}
            </div>

            {/* Region with debounce */}
            <div>
              <label htmlFor="region" className="block text-sm font-medium text-gray-700 mb-1">
                <i className="fas fa-globe mr-1"></i>
                Region
              </label>
              <input
                type="text"
                id="region"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Region"
                value={localRegion}
                onChange={handleRegionChange}
              />
              {localRegion !== filters.region && (
                <small className="text-gray-500 block mt-1">
                  <i className="fas fa-hourglass-half mr-1"></i>
                  Applying filter...
                </small>
              )}
            </div>

            {/* Price from */}
            <div>
              <label htmlFor="minPrice" className="block text-sm font-medium text-gray-700 mb-1">
                <i className="fas fa-euro-sign mr-1"></i>
                Price from
              </label>
              <input
                type="number"
                id="minPrice"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                min="0"
                placeholder="From"
                value={filters.minPrice === 0 ? '' : filters.minPrice}
                onChange={(e) =>
                  handleFilterChange('minPrice', e.target.value === '' ? 0 : Number(e.target.value))
                }
              />
            </div>

            {/* Price to */}
            <div>
              <label htmlFor="maxPrice" className="block text-sm font-medium text-gray-700 mb-1">
                <i className="fas fa-euro-sign mr-1"></i>
                Price to
              </label>
              <input
                type="number"
                id="maxPrice"
                className="w-full border border-gray-300 rounded px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                min="0"
                placeholder="To"
                value={filters.maxPrice === 10000 || filters.maxPrice === 0 ? '' : filters.maxPrice}
                onChange={(e) =>
                  handleFilterChange('maxPrice', e.target.value === '' ? 0 : Number(e.target.value))
                }
              />
            </div>

            {/* Clear button */}
            <div className="flex items-end">
              <button
                type="button"
                className="w-full border border-gray-400 text-gray-700 px-3 py-1.5 rounded text-sm hover:bg-gray-100 bg-transparent cursor-pointer"
                onClick={clearFilters}
              >
                <i className="fas fa-times mr-1"></i>
                Clear
              </button>
            </div>
          </div>

          {/* Activities */}
          <div className="mt-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              <i className="fas fa-running mr-1"></i>
              List Activities
            </label>
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-1">
              {activities.map((activity) => (
                <div key={activity.activityId} className="flex items-center">
                  <input
                    className="mr-2"
                    type="checkbox"
                    id={`activity-${activity.activityId}`}
                    checked={filters.activitiesIds.includes(activity.activityId ?? '')}
                    onChange={() => handleActivityToggle(activity.activityId ?? '')}
                  />
                  <label className="text-sm" htmlFor={`activity-${activity.activityId}`}>
                    {activity.name}
                  </label>
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
              <div className="bg-sky-50 border border-sky-200 text-sky-800 p-3 rounded flex items-center">
                <i className="fas fa-info-circle mr-2"></i>
                <span>
                  Active filters:{' '}
                  {filters.search && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
                      Search
                    </span>
                  )}
                  {filters.city && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
                      City
                    </span>
                  )}
                  {filters.region && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
                      Region
                    </span>
                  )}
                  {filters.minPrice > 0 && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
                      Min. price
                    </span>
                  )}
                  {filters.maxPrice < 10000 && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
                      Max. Price
                    </span>
                  )}
                  {filters.activitiesIds.length > 0 && (
                    <span className="inline-block bg-blue-600 text-white text-xs px-2 py-0.5 rounded mr-1">
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
  );
};
