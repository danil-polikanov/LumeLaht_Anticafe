import React, { useState } from 'react';
import { useGetMyBookingsQuery, useCancelBookingMutation } from '@/entities/booking';
import { useAppSelector } from '@/shared/lib/hooks/useRedux';
import { Navigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { BookingResponse } from '@/shared/types';

type TabFilter = 'upcoming' | 'past' | 'cancelled';

export const MyBookings: React.FC = () => {
  const token = useAppSelector((state) => state.auth.token);
  const { data: bookings, isLoading, error } = useGetMyBookingsQuery(undefined, { skip: !token });
  const [cancelBooking] = useCancelBookingMutation();
  const [activeTab, setActiveTab] = useState<TabFilter>('upcoming');

  if (!token) {
    return <Navigate to="/" replace />;
  }

  const handleCancel = async (bookingId: string) => {
    try {
      await cancelBooking(bookingId).unwrap();
      toast.success('Booking cancelled');
    } catch {
      toast.error('Failed to cancel booking');
    }
  };

  const formatDate = (dateStr: string) => {
    const d = new Date(dateStr);
    return d.toLocaleDateString('en-GB', { weekday: 'short', day: '2-digit', month: 'short' });
  };

  const formatTime = (dateStr: string) => {
    const d = new Date(dateStr);
    return d.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' });
  };

  const now = new Date();

  const filtered = (bookings ?? []).filter((b) => {
    if (activeTab === 'cancelled') return b.status.toLowerCase() === 'cancelled';
    if (activeTab === 'past') return new Date(b.startTime) < now && b.status.toLowerCase() !== 'cancelled';
    return new Date(b.startTime) >= now && b.status.toLowerCase() !== 'cancelled';
  });

  const tabCounts = {
    upcoming: (bookings ?? []).filter((b) => new Date(b.startTime) >= now && b.status.toLowerCase() !== 'cancelled').length,
    past: (bookings ?? []).filter((b) => new Date(b.startTime) < now && b.status.toLowerCase() !== 'cancelled').length,
    cancelled: (bookings ?? []).filter((b) => b.status.toLowerCase() === 'cancelled').length,
  };

  const statusConfig: Record<string, { bg: string; text: string; icon: string }> = {
    confirmed: { bg: 'bg-emerald-100', text: 'text-emerald-700', icon: 'fa-check-circle' },
    cancelled: { bg: 'bg-red-100', text: 'text-red-600', icon: 'fa-times-circle' },
    pending: { bg: 'bg-yellow-100', text: 'text-yellow-700', icon: 'fa-hourglass-half' },
  };

  const getStatus = (status: string) => statusConfig[status.toLowerCase()] ?? statusConfig.pending;

  const tabs: { key: TabFilter; label: string; icon: string }[] = [
    { key: 'upcoming', label: 'Upcoming', icon: 'fa-calendar-alt' },
    { key: 'past', label: 'Past', icon: 'fa-history' },
    { key: 'cancelled', label: 'Cancelled', icon: 'fa-ban' },
  ];

  const renderCard = (booking: BookingResponse) => {
    const sc = getStatus(booking.status);
    const isFuture = new Date(booking.startTime) > now;
    const canCancel = isFuture && booking.status.toLowerCase() !== 'cancelled';

    return (
      <div
        key={booking.bookingId}
        className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden hover:shadow-md transition-shadow"
      >
        <div className="p-5">
          <div className="flex items-start justify-between mb-3">
            <div className="flex-1 min-w-0">
              <h3 className="font-bold text-gray-800 text-lg mb-0.5 truncate">{booking.roomName}</h3>
              <div className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium ${sc.bg} ${sc.text}`}>
                <i className={`fas ${sc.icon} text-[10px]`}></i>
                {booking.status}
              </div>
            </div>
            <div className="text-right flex-shrink-0 ml-3">
              <div className="text-2xl font-bold text-accent">{booking.totalPrice.toFixed(2)}</div>
              <div className="text-xs text-gray-400">EUR</div>
            </div>
          </div>

          <div className="grid grid-cols-2 gap-3 bg-gray-50 rounded-xl p-3 mb-3">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-accent/10 flex items-center justify-center">
                <i className="fas fa-calendar text-accent text-xs"></i>
              </div>
              <div>
                <div className="text-[10px] text-gray-400 uppercase">Date</div>
                <div className="text-sm font-semibold text-gray-700">{formatDate(booking.startTime)}</div>
              </div>
            </div>
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-lg bg-accent/10 flex items-center justify-center">
                <i className="fas fa-clock text-accent text-xs"></i>
              </div>
              <div>
                <div className="text-[10px] text-gray-400 uppercase">Time</div>
                <div className="text-sm font-semibold text-gray-700">
                  {formatTime(booking.startTime)} — {formatTime(booking.endTime)}
                </div>
              </div>
            </div>
          </div>

          {canCancel && (
            <button
              onClick={() => handleCancel(booking.bookingId)}
              className="w-full py-2.5 rounded-xl text-sm font-medium text-red-500 border border-red-200 bg-red-50/50 hover:bg-red-100 hover:border-red-300 transition-all cursor-pointer flex items-center justify-center gap-2"
            >
              <i className="fas fa-times"></i>
              Cancel Booking
            </button>
          )}
        </div>
      </div>
    );
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-accent-50 via-white to-accent-50/30 pt-24 px-4 pb-10">
      <div className="max-w-3xl mx-auto">
        {/* Header */}
        <div className="mb-8">
          <div className="flex items-center gap-3 mb-2">
            <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-accent to-accent-hover flex items-center justify-center shadow-lg shadow-accent/20">
              <i className="fas fa-calendar-check text-white text-xl"></i>
            </div>
            <div>
              <h1 className="text-2xl font-bold text-gray-800 m-0">My Bookings</h1>
              <p className="text-sm text-gray-400 m-0">
                {(bookings ?? []).length} total booking{(bookings ?? []).length !== 1 ? 's' : ''}
              </p>
            </div>
          </div>
        </div>

        {/* Tabs */}
        <div className="flex bg-white rounded-2xl p-1.5 shadow-sm border border-gray-100 mb-6 gap-1">
          {tabs.map((t) => (
            <button
              key={t.key}
              onClick={() => setActiveTab(t.key)}
              className={`flex-1 flex items-center justify-center gap-2 py-2.5 rounded-xl text-sm font-semibold transition-all border-none cursor-pointer ${
                activeTab === t.key
                  ? 'bg-accent text-white shadow-md shadow-accent/20'
                  : 'bg-transparent text-gray-500 hover:text-gray-700 hover:bg-gray-50'
              }`}
            >
              <i className={`fas ${t.icon} text-xs`}></i>
              {t.label}
              {tabCounts[t.key] > 0 && (
                <span className={`text-[10px] px-1.5 py-0.5 rounded-full font-bold ${
                  activeTab === t.key ? 'bg-white/20 text-white' : 'bg-gray-200 text-gray-500'
                }`}>
                  {tabCounts[t.key]}
                </span>
              )}
            </button>
          ))}
        </div>

        {/* Content */}
        {isLoading && (
          <div className="flex flex-col items-center justify-center py-20">
            <div className="w-16 h-16 rounded-2xl bg-accent/10 flex items-center justify-center mb-4">
              <i className="fas fa-spinner fa-spin fa-2x text-accent"></i>
            </div>
            <p className="text-gray-400">Loading bookings...</p>
          </div>
        )}

        {error && (
          <div className="bg-red-50 border border-red-200 text-red-600 p-5 rounded-2xl text-center">
            <i className="fas fa-exclamation-circle mr-2"></i>
            Failed to load bookings. Please try again.
          </div>
        )}

        {!isLoading && !error && filtered.length === 0 && (
          <div className="text-center py-16">
            <div className="w-20 h-20 rounded-3xl bg-gray-100 flex items-center justify-center mx-auto mb-4">
              <i className={`fas ${activeTab === 'upcoming' ? 'fa-calendar-plus' : activeTab === 'past' ? 'fa-history' : 'fa-ban'} fa-2x text-gray-300`}></i>
            </div>
            <p className="text-lg font-semibold text-gray-400 mb-1">
              {activeTab === 'upcoming' ? 'No upcoming bookings' : activeTab === 'past' ? 'No past bookings' : 'No cancelled bookings'}
            </p>
            <p className="text-sm text-gray-300">
              {activeTab === 'upcoming' ? 'Browse rooms and make your first booking!' : 'Nothing to show here yet.'}
            </p>
          </div>
        )}

        {!isLoading && !error && filtered.length > 0 && (
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {filtered.map(renderCard)}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyBookings;
