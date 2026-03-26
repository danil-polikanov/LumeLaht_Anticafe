import React from 'react';
import { useSelector } from 'react-redux';
import { setSorting } from '@/entities/room/model';
import { selectSorting } from '@/entities/room/model';
import { useAppDispatch } from '@/shared/lib/hooks/useRedux';
import styles from './RoomSortPaggination.module.css';

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

  const btnClass = (field: string, position: 'first' | 'middle' | 'last') => {
    const posClass =
      position === 'first'
        ? styles.sortBtnFirst
        : position === 'last'
          ? styles.sortBtnLast
          : styles.sortBtnMiddle;
    const stateClass = sorting.field === field ? styles.sortBtnActive : styles.sortBtnInactive;
    return `${styles.sortBtn} ${posClass} ${stateClass}`;
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.sortGroup}>
        <span className={styles.sortLabel}>
          <i className="fas fa-sort-amount-down mr-1"></i>
          Sort by:
        </span>
        <div className={styles.btnGroup}>
          <button type="button" className={btnClass('name', 'first')} onClick={() => handleSortChange('name')}>
            <i className={`${getSortIcon('name')} mr-1`}></i>
            Name
          </button>
          <button type="button" className={btnClass('pricePerHour', 'middle')} onClick={() => handleSortChange('pricePerHour')}>
            <i className={`${getSortIcon('pricePerHour')} mr-1`}></i>
            Price
          </button>
          <button type="button" className={btnClass('city', 'last')} onClick={() => handleSortChange('city')}>
            <i className={`${getSortIcon('city')} mr-1`}></i>
            City
          </button>
        </div>
      </div>
    </div>
  );
};

export default RoomSortingAndPagination;
