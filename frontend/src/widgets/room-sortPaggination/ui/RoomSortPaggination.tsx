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
    <div className="d-flex flex-column flex-lg-row justify-content-between align-items-center mb-4">
      {/* Sorting */}
      <div className="d-flex align-items-center mb-3 mb-lg-0">
        <span className="me-3 text-muted">
          <i className="fas fa-sort-amount-down me-1"></i>
          Sort by:
        </span>
        <div className="btn-group" role="group">
          <button
            type="button"
            className={`btn btn-sm ${
              sorting.field === 'name' ? 'btn-primary' : 'btn-outline-primary'
            }`}
            onClick={() => handleSortChange('name')}
          >
            <i className={getSortIcon('name')} style={{ marginRight: '5px' }}></i>
            By name
          </button>
          <button
            type="button"
            className={`btn btn-sm ${
              sorting.field === 'pricePerHour' ? 'btn-primary' : 'btn-outline-primary'
            }`}
            onClick={() => handleSortChange('pricePerHour')}
          >
            <i className={getSortIcon('pricePerHour')} style={{ marginRight: '5px' }}></i>
            By price
          </button>
          <button
            type="button"
            className={`btn btn-sm ${
              sorting.field === 'city' ? 'btn-primary' : 'btn-outline-primary'
            }`}
            onClick={() => handleSortChange('city')}
          >
            <i className={getSortIcon('city')} style={{ marginRight: '5px' }}></i>
            By city
          </button>
        </div>
      </div>

      {/* Items per page second variant */}
      {/* <div className="d-flex align-items-center mb-3 mb-lg-0">
                <span className="me-2 text-muted">Show:</span>
                <select
                    className="form-select form-select-sm"
                    style={{ width: 'auto' }}
                    value={pagination.limit}
                    onChange={(e) => handleLimitChange(Number(e.target.value))}
                >
                    <option value={6}>6</option>
                    <option value={12}>12</option>
                    <option value={24}>24</option>
                    <option value={48}>48</option>
                </select>
                <span className="ms-2 text-muted">on page</span>
            </div> */}
      {/* Results info */}
      {/* <div className="text-muted small">
                <i className="fas fa-info-circle me-1"></i>
                Showing {Math.min(pagination.limit, pagination.total)} out of{' '}
                {pagination.total} results
            </div> */}
    </div>
  );
};

export default RoomSortingAndPagination;
