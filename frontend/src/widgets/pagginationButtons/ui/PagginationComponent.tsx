import React from 'react';
import { setPage } from '@/entities/room/model';
import { selectPagination } from '@/entities/room/model';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';

// Pagination component (extracted for reuse)
export const PaginationComponent: React.FC = () => {
  const dispatch = useAppDispatch();
  const pagination = useAppSelector(selectPagination);
  console.log('Pagination:', pagination);

  const handlePageChange = (page: number) => {
    dispatch(setPage(page));
  };

  const linkBase =
    'px-3 py-1.5 border border-gray-300 rounded text-blue-600 hover:bg-gray-100 cursor-pointer text-sm bg-white';
  const linkActive =
    'px-3 py-1.5 border border-blue-600 rounded bg-blue-600 text-white hover:bg-blue-700 cursor-pointer text-sm';

  const generatePaginationItems = () => {
    const items = [];
    const currentPage = pagination.currentPage;
    const totalPages = pagination.totalPages;

    // Previous page
    items.push(
      <li
        key="prev"
        className={`inline-block ${currentPage === 1 ? 'opacity-50 pointer-events-none' : ''}`}
      >
        <button
          className={linkBase}
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          <i className="fas fa-chevron-left"></i>
        </button>
      </li>,
    );

    // First page
    if (currentPage > 3) {
      items.push(
        <li key="first" className="inline-block">
          <button className={linkBase} onClick={() => handlePageChange(1)}>
            1
          </button>
        </li>,
      );
      if (currentPage > 4) {
        items.push(
          <li key="ellipsis1" className="inline-block opacity-50 pointer-events-none">
            <span className={linkBase}>...</span>
          </li>,
        );
      }
    }

    // Pages around current
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <li key={i} className="inline-block">
          <button
            className={i === currentPage ? linkActive : linkBase}
            onClick={() => handlePageChange(i)}
          >
            {i}
          </button>
        </li>,
      );
    }

    // Last page
    if (currentPage < totalPages - 2) {
      if (currentPage < totalPages - 3) {
        items.push(
          <li key="ellipsis2" className="inline-block opacity-50 pointer-events-none">
            <span className={linkBase}>...</span>
          </li>,
        );
      }
      items.push(
        <li key="last" className="inline-block">
          <button className={linkBase} onClick={() => handlePageChange(totalPages)}>
            {totalPages}
          </button>
        </li>,
      );
    }

    // Next page
    items.push(
      <li
        key="next"
        className={`inline-block ${currentPage === totalPages ? 'opacity-50 pointer-events-none' : ''}`}
      >
        <button
          className={linkBase}
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage === totalPages}
        >
          <i className="fas fa-chevron-right"></i>
        </button>
      </li>,
    );

    return items;
  };

  if (pagination.totalPages <= 1) return null;

  return (
    <div className="flex justify-center mt-4">
      <nav aria-label="Page navigation">
        <ul className="flex list-none gap-1 p-0 m-0">{generatePaginationItems()}</ul>
      </nav>
    </div>
  );
};

export default PaginationComponent;
