import React, { useState } from 'react';
import { RoomResponse } from '@/shared/types/room.types';

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

  return (
    <div className="fixed inset-0 flex justify-center items-center bg-black/80 z-[1050]">
      <div className="bg-white rounded-lg shadow-2xl flex flex-col max-w-[800px] w-[90%] max-h-[90vh]">
        {/* Header */}
        <div className="bg-blue-600 text-white p-4 rounded-t-lg flex justify-between items-center">
          <h5 className="mb-0 text-base font-semibold">
            <i className="fas fa-door-open mr-2"></i>
            {room.name}
          </h5>
          <button
            className="border border-white text-white px-2 py-1 rounded text-sm hover:bg-white/10 bg-transparent cursor-pointer"
            onClick={onClose}
          >
            <i className="fas fa-times"></i>
          </button>
        </div>

        <div className="p-4 overflow-auto flex-1">
          <div className="flex flex-wrap -mx-2">
            {/* Left column — images */}
            <div className="w-full md:w-1/2 px-2 mb-3">
              <div className="relative">
                {images.length > 0 ? (
                  <>
                    <img
                      src={images[currentImageIndex]}
                      alt={`Room ${currentImageIndex + 1}`}
                      className="max-w-full rounded w-full h-[250px] object-cover"
                    />
                    {images.length > 1 && (
                      <>
                        <button
                          className="bg-gray-800 text-white px-2 py-1 rounded text-sm hover:bg-gray-900 absolute top-1/2 left-2 -translate-y-1/2 border-none cursor-pointer"
                          onClick={prevImage}
                        >
                          <i className="fas fa-chevron-left"></i>
                        </button>
                        <button
                          className="bg-gray-800 text-white px-2 py-1 rounded text-sm hover:bg-gray-900 absolute top-1/2 right-2 -translate-y-1/2 border-none cursor-pointer"
                          onClick={nextImage}
                        >
                          <i className="fas fa-chevron-right"></i>
                        </button>
                      </>
                    )}
                    <div className="absolute bottom-2 right-2">
                      <span className="bg-gray-800 text-white px-2 py-0.5 rounded text-xs">
                        {currentImageIndex + 1} / {images.length}
                      </span>
                    </div>
                  </>
                ) : (
                  <div className="bg-gray-50 rounded flex items-center justify-center text-gray-500 h-[250px]">
                    <i className="fas fa-image fa-3x"></i>
                  </div>
                )}
              </div>
            </div>

            {/* Right column — info */}
            <div className="w-full md:w-1/2 px-2">
              <div className="mb-3">
                <h6 className="text-gray-500">Description</h6>
                <p className="mb-2">{room.description}</p>
              </div>

              <div className="mb-3">
                <h6 className="text-gray-500">Price</h6>
                <div className="flex items-center justify-center">
                  <span className="text-2xl font-bold text-green-600 mb-0">
                    {room.pricePerHour} €
                  </span>
                  <span className="text-gray-500 ml-2">/ hour</span>
                </div>
              </div>

              <div className="mb-3">
                <h6 className="text-gray-500">Status</h6>
                <span
                  className={`px-2 py-0.5 rounded text-xs text-white ${room.status ? 'bg-green-500' : 'bg-red-500'}`}
                >
                  <i className={`fas ${room.status ? 'fa-check' : 'fa-times'} mr-1`}></i>
                  {room.status ? 'Active' : 'Inactive'}
                </span>
              </div>
            </div>
          </div>

          {/* Address */}
          {room.address && (
            <div className="mb-3">
              <h6 className="text-gray-500">
                <i className="fas fa-map-marker-alt mr-1"></i>
                Address
              </h6>
              <div className="bg-gray-50 p-3 rounded">
                <div className="flex flex-wrap -mx-2">
                  <div className="w-full md:w-1/2 px-2">
                    <p className="mb-1">
                      <strong>{room.address.addressName}</strong>
                    </p>
                    <p className="mb-1">
                      {room.address.city}, {room.address.region}
                    </p>
                    <p className="mb-1">
                      {room.address.postalCode}, {room.address.country}
                    </p>
                  </div>
                  <div className="w-full md:w-1/2 px-2">
                    {room.address.phoneNumber && (
                      <p className="mb-1">
                        <i className="fas fa-phone mr-1"></i>
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
            <div className="mb-3">
              <h6 className="text-gray-500">
                <i className="fas fa-list mr-1"></i>
                Available activities
              </h6>
              <div className="flex flex-wrap -mx-2">
                {room.activity.map((activity) => (
                  <div key={activity.activityId} className="w-full md:w-1/2 px-2 mb-2">
                    <div className="border border-blue-600 rounded">
                      <div className="p-3">
                        <h6 className="font-semibold mb-1">{activity.name}</h6>
                        <p className="text-sm text-gray-500 mb-0">{activity.description}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* Actions */}
          <div className="flex gap-2 mt-4">
            <button className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 border-none cursor-pointer">
              <i className="fas fa-calendar-plus mr-1"></i>
              Book
            </button>
            <button className="border border-blue-600 text-blue-600 px-4 py-2 rounded hover:bg-blue-50 bg-transparent cursor-pointer">
              <i className="fas fa-heart mr-1"></i>Add to favourites
            </button>
            <button className="border border-gray-400 text-gray-700 px-4 py-2 rounded hover:bg-gray-100 bg-transparent cursor-pointer">
              <i className="fas fa-share mr-1"></i>
              Share
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RoomDetailOverlay;
