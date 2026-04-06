import React from 'react';
import { RoomResponse, ActivityResponse, AddressResponse } from '@/shared/types/room.types';
import styles from './RoomCard.module.css';

interface RoomCardProps {
  room: RoomResponse;
  onRoomClick: (roomId: string) => void;
}

export const RoomCard: React.FC<RoomCardProps> = ({ room, onRoomClick }) => {
  const handleCardClick = () => {
    if (room.roomId) {
      onRoomClick(room.roomId);
    }
  };

  const formatPrice = (price: number | undefined) => {
    if (!price) return 'Price not specified';
    return `${price.toLocaleString('de-DE')} €/hour`;
  };

  const getStatusBadge = (status: string | undefined) => {
    if (status === undefined) return null;
    const isActive = !!status;
    return (
      <span
        className={`${styles.statusBadge} ${isActive ? styles.statusActive : styles.statusInactive}`}
      >
        <span
          className={`${styles.statusDot} ${isActive ? styles.statusDotActive : styles.statusDotInactive}`}
        />
        {isActive ? 'Active' : 'Closed'}
      </span>
    );
  };

  const formatAddress = (address: AddressResponse | undefined) => {
    if (!address) return 'No info';
    const parts = [];
    if (address.city) parts.push(address.city);
    if (address.region) parts.push(address.region);
    if (address.addressName) parts.push(address.addressName);
    return parts.join(', ') || 'Address not specified';
  };

  const getActivityBadges = (activities: ActivityResponse[] | undefined) => {
    if (!activities || activities.length === 0) return null;
    return activities.slice(0, 3).map((activity, index) => (
      <span key={activity.activityId || index} className={styles.activityBadge}>
        {activity.name}
      </span>
    ));
  };

  const renderImage = () => {
    if (room.images && room.images.length > 0) {
      const mainImage = room.images.find((r) => r.isMain) ?? room.images[0];
      return mainImage?.url ? (
        <img src={mainImage.url} className={styles.image} alt="Room photo" />
      ) : (
        <div className={styles.imagePlaceholder}>
          <i className="fas fa-image fa-2x"></i>
        </div>
      );
    }
    return (
      <div className={styles.imagePlaceholder}>
        <i className="fas fa-image fa-2x"></i>
      </div>
    );
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.card} onClick={handleCardClick}>
        <div className={styles.imageWrap}>{renderImage()}</div>

        <div className={styles.body}>
          <div className={styles.titleRow}>
            <h5 className={styles.title}>{room.name || 'Untitled'}</h5>
            {getStatusBadge(room.status)}
          </div>

          <p className={styles.description}>
            {room.description
              ? room.description.length > 100
                ? `${room.description.substring(0, 100)}...`
                : room.description
              : 'No description'}
          </p>

          <div className="mb-3">
            <div className={styles.addressRow}>
              <i className={`fas fa-map-marker-alt ${styles.addressIcon}`}></i>
              <span>{formatAddress(room.address)}</span>
            </div>
            {room.address?.phoneNumber && (
              <div className={`${styles.addressRow} mt-1`}>
                <i className={`fas fa-phone ${styles.addressIcon}`}></i>
                <span>{room.address.phoneNumber}</span>
              </div>
            )}
          </div>

          {room.activity && room.activity.length > 0 && (
            <div className="mb-3">
              <div>
                {getActivityBadges(room.activity)}
                {room.activity.length > 3 && (
                  <span className={styles.activityOverflow}>+{room.activity.length - 3}</span>
                )}
              </div>
            </div>
          )}

          <div className={styles.footer}>
            <div className={styles.footerInner}>
              <div className={styles.price}>{formatPrice(room.pricePerHour)}</div>
              <button
                className={styles.readMoreBtn}
                onClick={(e) => {
                  e.stopPropagation();
                  handleCardClick();
                }}
              >
                <i className="fas fa-eye mr-1.5"></i>
                Read More
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
