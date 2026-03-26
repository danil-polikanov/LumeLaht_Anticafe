import React from 'react';
import { setPage } from '@/entities/room/model';
import { selectPagination } from '@/entities/room/model';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
import styles from './PagginationComponent.module.css';

export const PaginationComponent: React.FC = () => {
  const dispatch = useAppDispatch();
  const pagination = useAppSelector(selectPagination);

  const handlePageChange = (page: number) => {
    dispatch(setPage(page));
  };

  const generatePaginationItems = () => {
    const items = [];
    const currentPage = pagination.currentPage;
    const totalPages = pagination.totalPages;

    items.push(
      <li
        key="prev"
        className={`${styles.item} ${currentPage === 1 ? styles.itemDisabled : ''}`}
      >
        <button
          className={styles.link}
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={currentPage === 1}
        >
          <i className="fas fa-chevron-left"></i>
        </button>
      </li>,
    );

    if (currentPage > 3) {
      items.push(
        <li key="first" className={styles.item}>
          <button className={styles.link} onClick={() => handlePageChange(1)}>
            1
          </button>
        </li>,
      );
      if (currentPage > 4) {
        items.push(
          <li key="ellipsis1" className={`${styles.item} ${styles.itemDisabled}`}>
            <span className={styles.link}>...</span>
          </li>,
        );
      }
    }

    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <li key={i} className={styles.item}>
          <button
            className={i === currentPage ? styles.linkActive : styles.link}
            onClick={() => handlePageChange(i)}
          >
            {i}
          </button>
        </li>,
      );
    }

    if (currentPage < totalPages - 2) {
      if (currentPage < totalPages - 3) {
        items.push(
          <li key="ellipsis2" className={`${styles.item} ${styles.itemDisabled}`}>
            <span className={styles.link}>...</span>
          </li>,
        );
      }
      items.push(
        <li key="last" className={styles.item}>
          <button className={styles.link} onClick={() => handlePageChange(totalPages)}>
            {totalPages}
          </button>
        </li>,
      );
    }

    items.push(
      <li
        key="next"
        className={`${styles.item} ${currentPage === totalPages ? styles.itemDisabled : ''}`}
      >
        <button
          className={styles.link}
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
    <div className={styles.wrapper}>
      <nav aria-label="Page navigation">
        <ul className={styles.list}>{generatePaginationItems()}</ul>
      </nav>
    </div>
  );
};

export default PaginationComponent;
