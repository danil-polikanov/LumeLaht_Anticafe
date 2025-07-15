import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setSorting, setPage, setLimit } from '../redux/rooms/roomSlice';
import { selectSorting, selectPagination } from '../redux/rooms/roomsSelectors';
const RoomSortingAndPagination: React.FC = () => {
    const dispatch = useDispatch();
    const sorting = useSelector(selectSorting);
    const pagination = useSelector(selectPagination);

    const handleSortChange = (field: 'name' | 'pricePerHour' | 'city') => {
        const newDirection =
            sorting.field === field && sorting.direction === 'asc'
                ? 'desc'
                : 'asc';
        dispatch(setSorting({ field, direction: newDirection }));
    };

    const handleLimitChange = (limit: number) => {
        dispatch(setLimit(limit));
    };

    const handlePageChange = (page: number) => {
        dispatch(setPage(page));
    };

    const getSortIcon = (field: 'name' | 'pricePerHour' | 'city') => {
        if (sorting.field !== field) return 'fas fa-sort';
        return sorting.direction === 'asc'
            ? 'fas fa-sort-up'
            : 'fas fa-sort-down';
    };

    const generatePaginationItems = () => {
        const items = [];
        const maxVisiblePages = 5;
        const currentPage = pagination.page;
        const totalPages = pagination.totalPages;

        // Первая страница
        if (currentPage > 3) {
            items.push(
                <li key="first" className="page-item">
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(1)}
                    >
                        1
                    </button>
                </li>
            );
            if (currentPage > 4) {
                items.push(
                    <li key="ellipsis1" className="page-item disabled">
                        <span className="page-link">...</span>
                    </li>
                );
            }
        }

        // Страницы вокруг текущей
        const startPage = Math.max(1, currentPage - 2);
        const endPage = Math.min(totalPages, currentPage + 2);

        for (let i = startPage; i <= endPage; i++) {
            items.push(
                <li
                    key={i}
                    className={`page-item ${i === currentPage ? 'active' : ''}`}
                >
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(i)}
                    >
                        {i}
                    </button>
                </li>
            );
        }

        // Последняя страница
        if (currentPage < totalPages - 2) {
            if (currentPage < totalPages - 3) {
                items.push(
                    <li key="ellipsis2" className="page-item disabled">
                        <span className="page-link">...</span>
                    </li>
                );
            }
            items.push(
                <li key="last" className="page-item">
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(totalPages)}
                    >
                        {totalPages}
                    </button>
                </li>
            );
        }

        return items;
    };

    return (
        <div className="d-flex flex-column flex-lg-row justify-content-between align-items-center mb-4">
            {/* Сортировка */}
            <div className="d-flex align-items-center mb-3 mb-lg-0">
                <span className="me-3 text-muted">
                    <i className="fas fa-sort-amount-down me-1"></i>
                    Сортировать:
                </span>
                <div className="btn-group" role="group">
                    <button
                        type="button"
                        className={`btn btn-sm ${
                            sorting.field === 'name'
                                ? 'btn-primary'
                                : 'btn-outline-primary'
                        }`}
                        onClick={() => handleSortChange('name')}
                    >
                        <i
                            className={getSortIcon('name')}
                            style={{ marginRight: '5px' }}
                        ></i>
                        По названию
                    </button>
                    <button
                        type="button"
                        className={`btn btn-sm ${
                            sorting.field === 'pricePerHour'
                                ? 'btn-primary'
                                : 'btn-outline-primary'
                        }`}
                        onClick={() => handleSortChange('pricePerHour')}
                    >
                        <i
                            className={getSortIcon('pricePerHour')}
                            style={{ marginRight: '5px' }}
                        ></i>
                        По цене
                    </button>
                    <button
                        type="button"
                        className={`btn btn-sm ${
                            sorting.field === 'city'
                                ? 'btn-primary'
                                : 'btn-outline-primary'
                        }`}
                        onClick={() => handleSortChange('city')}
                    >
                        <i
                            className={getSortIcon('city')}
                            style={{ marginRight: '5px' }}
                        ></i>
                        По городу
                    </button>
                </div>
            </div>

            {/* Количество элементов на странице */}
            <div className="d-flex align-items-center mb-3 mb-lg-0">
                <span className="me-2 text-muted">Показать:</span>
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
                <span className="ms-2 text-muted">на странице</span>
            </div>

            {/* Информация о результатах */}
            <div className="text-muted small">
                <i className="fas fa-info-circle me-1"></i>
                Показано {Math.min(pagination.limit, pagination.total)} из{' '}
                {pagination.total} результатов
            </div>
        </div>
    );
};

// Компонент пагинации (отдельный для переиспользования)
export const PaginationComponent: React.FC = () => {
    const dispatch = useDispatch();
    const pagination = useSelector(selectPagination);

    const handlePageChange = (page: number) => {
        dispatch(setPage(page));
    };

    const generatePaginationItems = () => {
        const items = [];
        const currentPage = pagination.page;
        const totalPages = pagination.totalPages;

        // Предыдущая страница
        items.push(
            <li
                key="prev"
                className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}
            >
                <button
                    className="page-link"
                    onClick={() => handlePageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                >
                    <i className="fas fa-chevron-left"></i>
                </button>
            </li>
        );

        // Первая страница
        if (currentPage > 3) {
            items.push(
                <li key="first" className="page-item">
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(1)}
                    >
                        1
                    </button>
                </li>
            );
            if (currentPage > 4) {
                items.push(
                    <li key="ellipsis1" className="page-item disabled">
                        <span className="page-link">...</span>
                    </li>
                );
            }
        }

        // Страницы вокруг текущей
        const startPage = Math.max(1, currentPage - 2);
        const endPage = Math.min(totalPages, currentPage + 2);

        for (let i = startPage; i <= endPage; i++) {
            items.push(
                <li
                    key={i}
                    className={`page-item ${i === currentPage ? 'active' : ''}`}
                >
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(i)}
                    >
                        {i}
                    </button>
                </li>
            );
        }

        // Последняя страница
        if (currentPage < totalPages - 2) {
            if (currentPage < totalPages - 3) {
                items.push(
                    <li key="ellipsis2" className="page-item disabled">
                        <span className="page-link">...</span>
                    </li>
                );
            }
            items.push(
                <li key="last" className="page-item">
                    <button
                        className="page-link"
                        onClick={() => handlePageChange(totalPages)}
                    >
                        {totalPages}
                    </button>
                </li>
            );
        }

        // Следующая страница
        items.push(
            <li
                key="next"
                className={`page-item ${
                    currentPage === totalPages ? 'disabled' : ''
                }`}
            >
                <button
                    className="page-link"
                    onClick={() => handlePageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                >
                    <i className="fas fa-chevron-right"></i>
                </button>
            </li>
        );

        return items;
    };

    if (pagination.totalPages <= 1) return null;

    return (
        <div className="d-flex justify-content-center mt-4">
            <nav aria-label="Навигация по страницам">
                <ul className="pagination pagination-sm">
                    {generatePaginationItems()}
                </ul>
            </nav>
        </div>
    );
};

export default RoomSortingAndPagination;
