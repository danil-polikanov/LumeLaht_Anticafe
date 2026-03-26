import React, { useEffect, useState } from 'react';
import { RoomResponse } from '@/shared/types/room.types';
import { useAppSelector } from '@/shared/lib/hooks/useRedux';
import { useCreateBookingMutation, useGetRoomBookingsQuery } from '@/entities/booking';
import { AuthModal } from '@/widgets/auth-modal/ui/AuthModal';
import toast from 'react-hot-toast';
import styles from './RoomDetailOverlay.module.css';

export const RoomDetailOverlay: React.FC<{
  room: RoomResponse;
  onClose: () => void;
}> = ({ room, onClose }) => {
  const token = useAppSelector((state) => state.auth.token);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [showAuthModal, setShowAuthModal] = useState(false);
  const [selectedDate, setSelectedDate] = useState(() => {
    const today = new Date();
    return today.toISOString().split('T')[0];
  });
  const [selectedHour, setSelectedHour] = useState<number | null>(null);
  const [createBooking, { isLoading: isBooking }] = useCreateBookingMutation();

  const { data: roomBookings } = useGetRoomBookingsQuery(
    { roomId: room.roomId!, date: selectedDate },
    { skip: !room.roomId }
  );

  const bookedHours = new Set(
    roomBookings?.map((b) => new Date(b.startTime).getHours()) ?? []
  );

  const isToday = selectedDate === new Date().toISOString().split('T')[0];
  const currentHour = new Date().getHours();

  const timeSlots = Array.from({ length: 13 }, (_, i) => i + 9); // 9:00–21:00

  const handleBook = async () => {
    if (!token) {
      setShowAuthModal(true);
      return;
    }
    if (selectedHour === null) {
      toast.error('Please select a time slot');
      return;
    }
    const startTimeStr = `${selectedDate}T${String(selectedHour).padStart(2, '0')}:00:00`;
    try {
      await createBooking({ roomId: room.roomId!, startTime: startTimeStr }).unwrap();
      toast.success('Booking confirmed!');
      setSelectedHour(null);
    } catch (err: unknown) {
      const error = err as { data?: { message?: string; Message?: string } };
      toast.error(error.data?.message || error.data?.Message || 'Failed to book. Please try again.');
    }
  };

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

          {/* Booking */}
          <div className="mt-4 pt-4 border-t border-gray-100">
            <div className="flex items-center gap-2 mb-4">
              <div className="w-8 h-8 rounded-lg bg-accent/10 flex items-center justify-center">
                <i className="fas fa-calendar-plus text-accent text-sm"></i>
              </div>
              <h6 className="text-sm font-semibold text-gray-800 uppercase tracking-wide m-0">
                Book this room
              </h6>
            </div>

            {room.status !== 'Available' ? (
              <div className="bg-amber-50 border border-amber-200 text-amber-700 rounded-xl p-4 mb-4 flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-amber-100 flex items-center justify-center flex-shrink-0">
                  <i className="fas fa-exclamation-triangle text-amber-500"></i>
                </div>
                <div>
                  <p className="font-semibold text-sm mb-0.5">Room Unavailable</p>
                  <p className="text-xs mb-0">This room is currently <strong>{room.status}</strong> and cannot be booked.</p>
                </div>
              </div>
            ) : (
              <div className="bg-accent-50/30 rounded-xl border border-accent-100 p-4">
                {/* Date picker */}
                <div className="mb-4">
                  <label className="block text-xs font-medium text-gray-500 mb-2 uppercase tracking-wide">
                    <i className="fas fa-calendar-day mr-1"></i> Select Date
                  </label>
                  <input
                    type="date"
                    value={selectedDate}
                    min={new Date().toISOString().split('T')[0]}
                    onChange={(e) => {
                      setSelectedDate(e.target.value);
                      setSelectedHour(null);
                    }}
                    className="px-4 py-2.5 bg-white border border-accent-200 rounded-xl text-sm focus:ring-2 focus:ring-accent/30 focus:border-accent outline-none transition-all w-full max-w-[200px] cursor-pointer"
                  />
                </div>

                {/* Time slots grid */}
                <div className="mb-1">
                  <label className="block text-xs font-medium text-gray-500 mb-2 uppercase tracking-wide">
                    <i className="fas fa-clock mr-1"></i> Select Time
                  </label>
                  <div className="grid grid-cols-4 sm:grid-cols-5 gap-2">
                    {timeSlots.map((hour) => {
                      const isPast = isToday && hour <= currentHour;
                      const isBooked = bookedHours.has(hour) || isPast;
                      const isSelected = selectedHour === hour;
                      const endHour = hour + 1;
                      return (
                        <button
                          key={hour}
                          type="button"
                          disabled={isBooked}
                          onClick={() => setSelectedHour(hour)}
                          className={`relative flex flex-col items-center py-2.5 px-2 rounded-xl text-xs font-medium border-2 transition-all ${
                            isBooked
                              ? 'bg-gray-50 text-gray-300 border-gray-100 cursor-not-allowed'
                              : isSelected
                                ? 'bg-accent text-white border-accent shadow-md shadow-accent/20 scale-[1.03]'
                                : 'bg-white text-gray-700 border-gray-200 hover:border-accent hover:shadow-sm cursor-pointer'
                          }`}
                        >
                          <i className={`fas ${isBooked ? 'fa-ban' : isSelected ? 'fa-check-circle' : 'fa-clock'} text-[10px] mb-1 ${
                            isBooked ? 'text-gray-300' : isSelected ? 'text-white/80' : 'text-accent/50'
                          }`}></i>
                          <span className="font-bold text-sm">{String(hour).padStart(2, '0')}:00</span>
                          <span className={`text-[10px] ${isSelected ? 'text-white/70' : 'text-gray-400'}`}>
                            {String(hour).padStart(2, '0')}-{String(endHour).padStart(2, '0')}
                          </span>
                        </button>
                      );
                    })}
                  </div>
                </div>

                {/* Legend */}
                <div className="flex gap-4 mt-3 text-[10px] text-gray-400">
                  <span className="flex items-center gap-1">
                    <span className="w-3 h-3 rounded border-2 border-gray-200 bg-white inline-block"></span> Available
                  </span>
                  <span className="flex items-center gap-1">
                    <span className="w-3 h-3 rounded bg-gray-100 border-2 border-gray-100 inline-block"></span> Booked
                  </span>
                  <span className="flex items-center gap-1">
                    <span className="w-3 h-3 rounded bg-accent border-2 border-accent inline-block"></span> Selected
                  </span>
                </div>
              </div>
            )}

            <div className="flex gap-2 mt-4">
              <button
                className="flex-1 bg-gradient-to-r from-accent to-accent-hover text-white px-5 py-3 rounded-xl hover:shadow-lg hover:shadow-accent/25 border-none cursor-pointer transition-all font-semibold disabled:opacity-40 disabled:cursor-not-allowed disabled:shadow-none flex items-center justify-center gap-2"
                onClick={handleBook}
                disabled={isBooking || room.status !== 'Available'}
              >
                <i className={`fas ${isBooking ? 'fa-spinner fa-spin' : 'fa-calendar-plus'}`}></i>
                {!token ? 'Sign in to Book' : isBooking ? 'Booking...' : selectedHour !== null ? `Book ${String(selectedHour).padStart(2, '0')}:00 — ${String(selectedHour + 1).padStart(2, '0')}:00` : 'Select a time slot'}
              </button>
              <button className={styles.favBtn}>
                <i className="fas fa-heart"></i>
              </button>
              <button className={styles.shareBtn}>
                <i className="fas fa-share"></i>
              </button>
            </div>
          </div>

          {showAuthModal && <AuthModal onClose={() => setShowAuthModal(false)} />}
        </div>
      </div>
    </div>
  );
};

export default RoomDetailOverlay;
