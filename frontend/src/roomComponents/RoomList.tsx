import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
    selectRooms,
    selectSelectedRoom,
    selectLoading,
    selectError,
} from '../redux/rooms/roomsSelectors';
import { fetchRooms, fetchRoomById } from '../api/roomsThunks';
import RoomFilters from './RoomFilters';
import RoomSortingAndPagination from './RoomSortPagination';
import RoomCard from './RoomCard';
import { AppDispatch } from '../redux/store';
import AdditionalInfo from './AdditionalInfo';
import PaginationComponent from './PaginationComponent';
import RoomDetailOverlay from './RoomDetailOverlay';
import { setSelectedRoom } from '../redux/rooms/roomSlice';
const RoomsList: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const rooms = useSelector(selectRooms);
    const selectedRoomDetail = useSelector(selectSelectedRoom);

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
    const handleCloseDetail = () => {
        dispatch(setSelectedRoom(null));
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
                            <span className="visually-hidden">Loading...</span>
                        </div>
                        <div className="mt-3 text-muted">Room loading...</div>
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
                        <strong>Loading error!</strong> {error}
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
                            Room catalog
                        </h2>
                        <button
                            className="btn btn-success"
                            onClick={() => console.log('Add new room')}
                        >
                            <i className="fas fa-plus me-1"></i>
                            Add Room
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
                                    Loading...
                                </span>
                            </div>
                            <span className="text-muted">
                                Updating results...
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
                                <h4>Rooms can't be found</h4>
                                <p>
                                    Try changing the filter settings or clear
                                    all filters
                                </p>
                            </div>
                        </div>
                    </div>
                )}
            </div>
            {/* Детальная информация о комнате */}
            {selectedRoomDetail && (
                <RoomDetailOverlay
                    room={selectedRoomDetail || selectedRoomDetail}
                    onClose={handleCloseDetail}
                />
            )}
            {/* Дополнительная информация <AdditionalInfo></AdditionalInfo> */}
            <PaginationComponent></PaginationComponent>
        </div>
    );
};

export default RoomsList;
