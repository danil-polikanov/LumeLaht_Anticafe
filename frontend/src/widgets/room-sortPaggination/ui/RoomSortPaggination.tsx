import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setSorting, setPage, setLimit } from '@/entities/room/model';
import { selectSorting, selectPagination } from '@/entities/room/model';
import { useAppDispatch } from '@/shared/lib/hooks/useRedux';
export const RoomSortingAndPagination: React.FC = () => {
  const dispatch = useAppDispatch();
  const sorting = useSelector(selectSorting);
  const pagination = useSelector(selectPagination);

  const handleSortChange = (field: 'name' | 'pricePerHour' | 'city') => {
    const newDirection = sorting.field === field && sorting.direction === 'asc' ? 'desc' : 'asc';
    dispatch(setSorting({ field, direction: newDirection }));
  };
  const handlePageChange = (page: number) => {
    dispatch(setPage(page));
  };

  const getSortIcon = (field: 'name' | 'pricePerHour' | 'city') => {
    if (sorting.field !== field) return 'fas fa-sort';
    return sorting.direction === 'asc' ? 'fas fa-sort-up' : 'fas fa-sort-down';
  };

  const generatePaginationItems = () => {
    const items = [];
    const maxVisiblePages = 5;
    const currentPage = pagination.currentPage;
    const totalPages = pagination.totalPages;

    // ÐŸÐµÑ€Ð²Ð°Ñ ÑÑ‚Ñ€Ð°Ð½Ð¸Ñ†Ð°
    if (currentPage > 3) {
      items.push(
        <li key="first" className="page-item">
          <button className="page-link" onClick={() => handlePageChange(1)}>
            1
          </button>
        </li>,
      );
      if (currentPage > 4) {
        items.push(
          <li key="ellipsis1" className="page-item disabled">
            <span className="page-link">...</span>
          </li>,
        );
      }
    }

    // Ð¡Ñ‚Ñ€Ð°Ð½Ð¸Ñ†Ñ‹ Ð²Ð¾ÐºÑ€ÑƒÐ³ Ñ‚ÐµÐºÑƒÑ‰ÐµÐ¹
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <li key={i} className={`page-item ${i === currentPage ? 'active' : ''}`}>
          <button className="page-link" onClick={() => handlePageChange(i)}>
            {i}
          </button>
        </li>,
      );
    }

    // ÐŸÐ¾ÑÐ»ÐµÐ´Ð½ÑÑ ÑÑ‚Ñ€Ð°Ð½Ð¸Ñ†Ð°
    if (currentPage < totalPages - 2) {
      if (currentPage < totalPages - 3) {
        items.push(
          <li key="ellipsis2" className="page-item disabled">
            <span className="page-link">...</span>
          </li>,
        );
      }
      items.push(
        <li key="last" className="page-item">
          <button className="page-link" onClick={() => handlePageChange(totalPages)}>
            {totalPages}
          </button>
        </li>,
      );
    }

    return items;
  };

  return (
    <div className="d-flex flex-column flex-lg-row justify-content-between align-items-center mb-4">
      {/* Ð¡Ð¾Ñ€Ñ‚Ð¸Ñ€Ð¾Ð²ÐºÐ° */}
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

      {/* ÐšÐ¾Ð»Ð¸Ñ‡ÐµÑÑ‚Ð²Ð¾ ÑÐ»ÐµÐ¼ÐµÐ½Ñ‚Ð¾Ð² Ð½Ð° ÑÑ‚Ñ€Ð°Ð½Ð¸Ñ†Ðµ second varient */}
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
      {/* Ð˜Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ñ Ð¾ Ñ€ÐµÐ·ÑƒÐ»ÑŒÑ‚Ð°Ñ‚Ð°Ñ… */}
      {/* <div className="text-muted small">
                <i className="fas fa-info-circle me-1"></i>
                Showing {Math.min(pagination.limit, pagination.total)} out of{' '}
                {pagination.total} results
            </div> */}
    </div>
  );
};

export default RoomSortingAndPagination;
