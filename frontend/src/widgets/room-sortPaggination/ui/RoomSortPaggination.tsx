import React from 'react';
import { useSelector } from 'react-redux';
import { setSorting } from '@/entities/room/model';
import { selectSorting } from '@/entities/room/model';
import { useAppDispatch } from '@/shared/lib/hooks/useRedux';

export const RoomSortingAndPagination: React.FC = () => {
  const dispatch = useAppDispatch();
  const sorting = useSelector(selectSorting);

  const handleSortChange = (field: 'name' | 'pricePerHour' | 'city') => {
    const newDirection = sorting.field === field && sorting.direction === 'asc' ? 'desc' : 'asc';
    dispatch(setSorting({ field, direction: newDirection }));
  };

  const getSortIcon = (field: 'name' | 'pricePerHour' | 'city') => {
    if (sorting.field !== field) return 'fas fa-sort';
    return sorting.direction === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down';
  };

  return (
    <div className="flex flex-col lg:flex-row justify-between items-center mb-4">
      {/* Sorting */}
      <div className="flex items-center mb-3 lg:mb-0">
        <span className="mr-3 text-gray-500">
          <i className="fas fa-sort-amount-down mr-1"></i>
          Sort by:
        </span>
        <div className="flex">
          <button
            type="button"
            className={`px-3 py-1 text-sm border ${
              sorting.field === 'name'
                ? 'bg-blue-600 text-white border-blue-600 hover:bg-blue-700'
                : 'border-blue-600 text-blue-600 hover:bg-blue-50 bg-transparent'
            } rounded-l cursor-pointer`}
            onClick={() => handleSortChange('name')}
          >
            <i className={`${getSortIcon('name')} mr-1`}></i>
            By name
          </button>
          <button
            type="button"
            className={`px-3 py-1 text-sm border-t border-b ${
              sorting.field === 'pricePerHour'
                ? 'bg-blue-600 text-white border-blue-600 hover:bg-blue-700'
                : 'border-blue-600 text-blue-600 hover:bg-blue-50 bg-transparent'
            } cursor-pointer`}
            onClick={() => handleSortChange('pricePerHour')}
          >
            <i className={`${getSortIcon('pricePerHour')} mr-1`}></i>
            By price
          </button>
          <button
            type="button"
            className={`px-3 py-1 text-sm border ${
              sorting.field === 'city'
                ? 'bg-blue-600 text-white border-blue-600 hover:bg-blue-700'
                : 'border-blue-600 text-blue-600 hover:bg-blue-50 bg-transparent'
            } rounded-r cursor-pointer`}
            onClick={() => handleSortChange('city')}
          >
            <i className={`${getSortIcon('city')} mr-1`}></i>
            By city
          </button>
        </div>
      </div>
    </div>
  );
};

export default RoomSortingAndPagination;
