import React from 'react';
import { setSorting, setPage, setLimit } from '@/entities/room/model';
import { selectSorting, selectPagination } from '@/entities/room/model';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
// Компонент пагинации (отдельный для переиспользования)
export const PaginationComponent: React.FC = () => {
  const dispatch = useAppDispatch();
  const pagination = useAppSelector(selectPagination);
  console.log('Pagination:', pagination);
  const handlePageChange = (page: number) => {
    dispatch(setPage(page));
  };
  const generatePaginationItems = () => {
    const items = [];
    const currentPage = pagination.currentPage;
    const totalPages = pagination.totalPages;

    // Предыдущая страница
    items.push(
      <li key="prev" className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
        <button
          className="page-link"
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          <i className="fas fa-chevron-left"></i>
        </button>
      </li>,
    );

    // Первая страница
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

    // Страницы вокруг текущей
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

    // Последняя страница
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

    // Следующая страница
    items.push(
      <li key="next" className={`page-item ${currentPage === totalPages ? 'disabled' : ''}`}>
        <button
          className="page-link"
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
    <div className="d-flex justify-content-center mt-4">
      <nav aria-label="Page navigation">
        <ul className="pagination ">{generatePaginationItems()}</ul>
      </nav>
    </div>
  );
};
export default PaginationComponent;
