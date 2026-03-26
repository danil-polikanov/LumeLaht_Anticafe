import React, { useEffect, useState } from 'react';
import { RoomResponse } from '@/shared/types/room.types';
import styles from './RoomDetailOverlay.module.css';

export const RoomDetailOverlay: React.FC<{
  room: RoomResponse;
  onClose: () => void;
}> = ({ room, onClose }) => {
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  const images = room.images?.map((img) => img.url) ?? [];

  const nextImage = () => {
    setCurrentImageIndex((prev) => (prev + 1) % images.length);
  };

  const prevImage = () => {
    setCurrentImageIndex((prev) => (prev - 1 + images.length) % images.length);
  };

  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [onClose]);

  useEffect(() => {
    document.body.style.overflow = 'hidden';
    return () => {
      document.body.style.overflow = '';
    };
  }, []);

  const handleBackdropClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) onClose();
  };

  return (
    <div className={styles.backdrop} onClick={handleBackdropClick}>
      <div className={styles.modal}>
        {/* Header */}
        <div className={styles.header}>
          <h5 className={styles.headerTitle}>
            <i className="fas fa-door-open mr-2"></i>
            {room.name}
          </h5>
          <button className={styles.closeBtn} onClick={onClose}>
            <i className="fas fa-times"></i>
          </button>
        </div>

        <div className={styles.body}>
          <div className={styles.twoCol}>
            {/* Images */}
            <div className={styles.colLeft}>
              <div className={styles.imageContainer}>
                {images.length > 0 ? (
                  <>
                    <img
                      src={images[currentImageIndex]}
                      alt={`Room ${currentImageIndex + 1}`}
                      className={styles.mainImage}
                    />
                    {images.length > 1 && (
                      <>
                        <button className={`${styles.navBtn} ${styles.navBtnLeft}`} onClick={prevImage}>
                          <i className="fas fa-chevron-left text-sm"></i>
                        </button>
                        <button className={`${styles.navBtn} ${styles.navBtnRight}`} onClick={nextImage}>
                          <i className="fas fa-chevron-right text-sm"></i>
                        </button>
                      </>
                    )}
                    <div className={styles.imageCounter}>
                      <span className={styles.imageCounterBadge}>
                        {currentImageIndex + 1} / {images.length}
                      </span>
                    </div>
                  </>
                ) : (
                  <div className={styles.imagePlaceholder}>
                    <i className="fas fa-image fa-3x"></i>
                  </div>
                )}
              </div>
            </div>

            {/* Info */}
            <div className={styles.colRight}>
              <div className="mb-4">
                <h6 className={styles.sectionLabel}>Description</h6>
                <p className={styles.descriptionText}>{room.description}</p>
              </div>

              <div className="mb-4">
                <h6 className={styles.sectionLabel}>Price</h6>
                <div className="flex items-baseline gap-1">
                  <span className={styles.priceValue}>{room.pricePerHour} €</span>
                  <span className={styles.priceUnit}>/ hour</span>
                </div>
              </div>

              <div className="mb-4">
                <h6 className={styles.sectionLabel}>Status</h6>
                <span
                  className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${
                    room.status ? 'bg-emerald-100 text-emerald-700' : 'bg-red-100 text-red-700'
                  }`}
                >
                  <span
                    className={`w-2 h-2 rounded-full mr-2 ${room.status ? 'bg-emerald-500' : 'bg-red-500'}`}
                  />
                  {room.status ? 'Active' : 'Inactive'}
                </span>
              </div>
            </div>
          </div>

          {/* Address */}
          {room.address && (
            <div className="mb-4">
              <h6 className={styles.sectionLabelSpaced}>
                <i className="fas fa-map-marker-alt mr-1"></i>
                Address
              </h6>
              <div className={styles.addressCard}>
                <div className="flex flex-wrap -mx-2">
                  <div className="w-full md:w-1/2 px-2">
                    <p className="mb-1 font-medium text-gray-800">{room.address.addressName}</p>
                    <p className="mb-1 text-gray-600">
                      {room.address.city}, {room.address.region}
                    </p>
                    <p className="mb-1 text-gray-600">
                      {room.address.postalCode}, {room.address.country}
                    </p>
                  </div>
                  <div className="w-full md:w-1/2 px-2">
                    {room.address.phoneNumber && (
                      <p className="mb-1 text-gray-600">
                        <i className="fas fa-phone mr-1 text-accent-400"></i>
                        {room.address.phoneNumber}
                      </p>
                    )}
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Activities */}
          {room.activity && room.activity.length > 0 && (
            <div className="mb-4">
              <h6 className={styles.sectionLabelSpaced}>
                <i className="fas fa-list mr-1"></i>
                Available activities
              </h6>
              <div className="flex flex-wrap -mx-1">
                {room.activity.map((activity) => (
                  <div key={activity.activityId} className="w-full md:w-1/2 px-1 mb-2">
                    <div className={styles.activityCard}>
                      <h6 className="font-semibold mb-1 text-gray-800">{activity.name}</h6>
                      <p className="text-sm text-gray-500 mb-0">{activity.description}</p>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* Actions */}
          <div className={styles.actions}>
            <button className={styles.bookBtn}>
              <i className="fas fa-calendar-plus mr-1.5"></i>
              Book
            </button>
            <button className={styles.favBtn}>
              <i className="fas fa-heart mr-1.5"></i>
              Favourite
            </button>
            <button className={styles.shareBtn}>
              <i className="fas fa-share mr-1.5"></i>
              Share
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RoomDetailOverlay;
