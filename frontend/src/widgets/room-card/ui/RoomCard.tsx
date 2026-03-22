import React from 'react';
import { RoomResponse, ActivityResponse, AddressResponse } from '@/shared/types/room.types';

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
    return (
      <span
        className={`inline-block text-white text-xs px-2 py-0.5 rounded ${status ? 'bg-green-500' : 'bg-red-500'}`}
      >
        {status ? 'Active' : 'Closed'}
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
      <span
        key={activity.activityId || index}
        className="inline-block bg-sky-400 text-white text-xs px-2 py-0.5 rounded mr-1"
      >
        {activity.name}
      </span>
    ));
  };

  const renderImage = () => {
    if (room.images && room.images.length > 0) {
      const mainImage = room.images.find((r) => r.isMain) ?? room.images[0];
      return mainImage?.url ? (
        <img src={mainImage.url} className="w-full h-[200px] object-cover block" alt="Room photo" />
      ) : (
        <div>Image unavailable</div>
      );
    }
    return <div>No images</div>;
  };

  return (
    <div className="w-full md:w-1/2 lg:w-1/3 px-2 mb-4">
      <div
        className="bg-white rounded-lg shadow-sm h-full flex flex-col transition-all duration-200 cursor-pointer hover:-translate-y-0.5 hover:shadow-md"
        onClick={handleCardClick}
      >
        {/* Room image */}
        <div className="bg-gray-100 flex items-center justify-center h-[200px]">
          <div className="text-center text-gray-500">{renderImage()}</div>
        </div>

        <div className="p-4 flex flex-col flex-1">
          {/* Title and status */}
          <div className="flex justify-between items-start mb-2">
            <h5 className="text-lg font-semibold mb-0 cursor-pointer">{room.name || 'Untitled'}</h5>
            {getStatusBadge(room.status)}
          </div>

          {/* Description */}
          <p className="text-gray-500 text-sm mb-3">
            {room.description
              ? room.description.length > 100
                ? `${room.description.substring(0, 100)}...`
                : room.description
              : 'No description'}
          </p>

          {/* Address */}
          <div className="mb-3">
            <div className="flex items-center text-gray-500 text-sm">
              <i className="fas fa-map-marker-alt mr-2"></i>
              <span>{formatAddress(room.address)}</span>
            </div>
            {room.address?.phoneNumber && (
              <div className="flex items-center text-gray-500 text-sm mt-1">
                <i className="fas fa-phone mr-2"></i>
                <span>{room.address.phoneNumber}</span>
              </div>
            )}
          </div>

          {/* Activities */}
          {room.activity && room.activity.length > 0 && (
            <div className="mb-3">
              <div className="text-sm text-gray-500 mb-1">Available activities:</div>
              <div>
                {getActivityBadges(room.activity)}
                {room.activity.length > 3 && (
                  <span className="inline-block bg-gray-500 text-white text-xs px-2 py-0.5 rounded">
                    +{room.activity.length - 3}
                  </span>
                )}
              </div>
            </div>
          )}

          {/* Price and button */}
          <div className="mt-auto">
            <div className="flex justify-between items-center">
              <div className="text-blue-600 font-bold">{formatPrice(room.pricePerHour)}</div>
              <button
                className="bg-blue-600 text-white px-3 py-1 rounded text-sm hover:bg-blue-700 transition-colors"
                onClick={(e) => {
                  e.stopPropagation();
                  handleCardClick();
                }}
              >
                <i className="fas fa-eye mr-1"></i>
                Read More
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
