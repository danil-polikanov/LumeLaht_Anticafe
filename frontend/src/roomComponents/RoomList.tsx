import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    selectRooms,
    selectLoading,
    selectError,
} from '../redux/rooms/roomsSelectors';
import { fetchRooms, fetchRoomById } from '../api/roomsThunks';
import RoomFilters from './RoomFilters';
import RoomSortingAndPagination from './RoomSortPagination';
import RoomCard from './RoomCard';
import { AppDispatch } from '../redux/store';

const RoomsList: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const rooms = useSelector(selectRooms);
    const loading = useSelector(selectLoading);
    const error = useSelector(selectError);

    useEffect(() => {
        dispatch(fetchRooms());
    }, [dispatch]);

    const handleRoomClick = (roomId: number) => {
        dispatch(fetchRoomById(roomId));
        // Здесь можно добавить навигацию к детальной странице комнаты
        // например, с помощью React Router
        console.log('Navigate to room detail:', roomId);
    };

    if (loading && rooms.length === 0) {
        return (
            <div className="container-fluid py-4">
                <div
                    className="d-flex justify-content-center align-items-center"
                    style={{ minHeight: '400px' }}
                >
                    <div className="text-center">
                        <div
                            className="spinner-border text-primary"
                            role="status"
                        >
                            <span className="visually-hidden">Загрузка...</span>
                        </div>
                        <div className="mt-3 text-muted">
                            Загрузка комнат...
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="container-fluid py-4">
                <div
                    className="alert alert-danger d-flex align-items-center"
                    role="alert"
                >
                    <i className="fas fa-exclamation-triangle me-2"></i>
                    <div>
                        <strong>Ошибка загрузки!</strong> {error}
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="container-fluid py-4">
            {/* Заголовок */}
            <div className="row mb-4">
                <div className="col-12">
                    <div className="d-flex justify-content-between align-items-center">
                        <h2 className="mb-0">
                            <i className="fas fa-building me-2 text-primary"></i>
                            Каталог комнат
                        </h2>
                        <button
                            className="btn btn-success"
                            onClick={() => console.log('Add new room')}
                        >
                            <i className="fas fa-plus me-1"></i>
                            Добавить комнату
                        </button>
                    </div>
                </div>
            </div>

            {/* Фильтры */}
            <div className="row mb-4">
                <div className="col-12">
                    <RoomFilters />
                </div>
            </div>

            {/* Сортировка и информация */}
            <div className="row mb-4">
                <div className="col-12">
                    <RoomSortingAndPagination />
                </div>
            </div>

            {/* Индикатор загрузки при фильтрации */}
            {loading && (
                <div className="row mb-3">
                    <div className="col-12">
                        <div className="d-flex justify-content-center">
                            <div
                                className="spinner-border spinner-border-sm text-primary me-2"
                                role="status"
                            >
                                <span className="visually-hidden">
                                    Загрузка...
                                </span>
                            </div>
                            <span className="text-muted">
                                Обновление результатов...
                            </span>
                        </div>
                    </div>
                </div>
            )}

            {/* Список комнат */}
            <div className="row">
                {rooms.length > 0 ? (
                    rooms.map((room) => (
                        <RoomCard
                            key={room.roomId}
                            room={room}
                            onRoomClick={handleRoomClick}
                        />
                    ))
                ) : (
                    <div className="col-12">
                        <div className="text-center py-5">
                            <div className="text-muted">
                                <i className="fas fa-search fa-3x mb-3"></i>
                                <h4>Комнаты не найдены</h4>
                                <p>
                                    Попробуйте изменить параметры фильтрации или
                                    очистить все фильтры
                                </p>
                            </div>
                        </div>
                    </div>
                )}
            </div>

            {/* Пагинация */}
            <div className="row">
                <div className="col-12">
                    <RoomSortingAndPagination />
                </div>
            </div>

            {/* Дополнительная информация */}
            <div className="row mt-4">
                <div className="col-12">
                    <div className="card bg-light">
                        <div className="card-body">
                            <div className="row text-center">
                                <div className="col-md-3">
                                    <div className="d-flex justify-content-center align-items-center mb-2">
                                        <i className="fas fa-shield-alt fa-2x text-success me-2"></i>
                                        <div>
                                            <strong>Безопасность</strong>
                                            <div className="small text-muted">
                                                Проверенные объекты
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex justify-content-center align-items-center mb-2">
                                        <i className="fas fa-clock fa-2x text-primary me-2"></i>
                                        <div>
                                            <strong>24/7</strong>
                                            <div className="small text-muted">
                                                Поддержка клиентов
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex justify-content-center align-items-center mb-2">
                                        <i className="fas fa-star fa-2x text-warning me-2"></i>
                                        <div>
                                            <strong>Качество</strong>
                                            <div className="small text-muted">
                                                Лучшие условия
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div className="col-md-3">
                                    <div className="d-flex justify-content-center align-items-center mb-2">
                                        <i className="fas fa-handshake fa-2x text-info me-2"></i>
                                        <div>
                                            <strong>Гарантия</strong>
                                            <div className="small text-muted">
                                                Возврат средств
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RoomsList;
