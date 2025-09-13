import React from 'react';
import { RoomResponse } from '../types/roomTypes/RoomResponse';
import styles from './RoomList.module.css';
interface RoomCardProps {
    room: RoomResponse;
    onRoomClick: (roomId: string) => void;
}

const RoomCard: React.FC<RoomCardProps> = ({ room, onRoomClick }) => {
    const handleCardClick = () => {
        if (room.roomId) {
            onRoomClick(room.roomId);
        }
    };

    const formatPrice = (price: number | undefined) => {
        if (!price) return 'Price not specified';
        return `${price.toLocaleString('de-DE')} €/час`;
    };

    const getStatusBadge = (status: string | undefined) => {
        if (status === undefined) return null;
        return (
            <span className={`badge ${status ? 'bg-success' : 'bg-danger'}`}>
                {status ? 'Active' : 'Close'}
            </span>
        );
    };

    const formatAddress = (address: any) => {
        if (!address) return 'Адрес не указан';
        const parts = [];
        if (address.city) parts.push(address.city);
        if (address.region) parts.push(address.region);
        if (address.addressName) parts.push(address.addressName);
        return parts.join(', ') || 'Address not specified';
    };

    const getActivityBadges = (activities: any[] | undefined) => {
        if (!activities || activities.length === 0) return null;
        return activities.slice(0, 3).map((activity, index) => (
            <span
                key={activity.activityId || index}
                className="badge bg-info me-1"
            >
                {activity.name}
            </span>
        ));
    };

    return (
        <div className="col-md-6 col-lg-4 mb-4">
            <div
                className={`card h-100 shadow-sm ${styles.room_card}`}
                onClick={handleCardClick}
            >
                {/* Изображение комнаты (заглушка) */}
                <div
                    className="card-img-top bg-light d-flex align-items-center justify-content-center"
                    style={{ height: '200px', cursor: 'pointer' }}
                >
                    <div className="text-center text-muted">
                        <i className="fas fa-image fa-3x mb-2"></i>
                        <div>Room photo</div>
                    </div>
                </div>

                <div className="card-body d-flex flex-column">
                    {/* Заголовок и статус */}
                    <div className="d-flex justify-content-between align-items-start mb-2">
                        <h5
                            className="card-title mb-0"
                            style={{ cursor: 'pointer' }}
                        >
                            {room.name || 'Untitled'}
                        </h5>
                        {getStatusBadge(room.status)}
                    </div>

                    {/* Описание */}
                    <p className="card-text text-muted small mb-3">
                        {room.description
                            ? room.description.length > 100
                                ? `${room.description.substring(0, 100)}...`
                                : room.description
                            : 'Untitled Description'}
                    </p>

                    {/* Адрес */}
                    <div className="mb-3">
                        <div className="d-flex align-items-center text-muted small">
                            <i className="fas fa-map-marker-alt me-2"></i>
                            <span>{formatAddress(room.address)}</span>
                        </div>
                        {room.address?.phoneNumber && (
                            <div className="d-flex align-items-center text-muted small mt-1">
                                <i className="fas fa-phone me-2"></i>
                                <span>{room.address.phoneNumber}</span>
                            </div>
                        )}
                    </div>

                    {/* Активности */}
                    {room.activity && room.activity.length > 0 && (
                        <div className="mb-3">
                            <div className="small text-muted mb-1">
                                Available activities:
                            </div>
                            <div>
                                {getActivityBadges(room.activity)}
                                {room.activity.length > 3 && (
                                    <span className="badge bg-secondary">
                                        +{room.activity.length - 3}
                                    </span>
                                )}
                            </div>
                        </div>
                    )}

                    {/* Цена и кнопка */}
                    <div className="mt-auto">
                        <div className="d-flex justify-content-between align-items-center">
                            <div className="text-primary fw-bold">
                                {formatPrice(room.pricePerHour)}
                            </div>
                            <button
                                className="btn btn-primary btn-sm"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    handleCardClick();
                                }}
                            >
                                <i className="fas fa-eye me-1"></i>
                                Read More
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RoomCard;
