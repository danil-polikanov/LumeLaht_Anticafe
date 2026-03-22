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
    <div
      className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center"
      style={{ backgroundColor: 'rgba(0,0,0,0.8)', zIndex: 1050 }}
    >
      <div
        className="card shadow-lg"
        style={{ maxWidth: '800px', width: '90%', maxHeight: '90vh' }}
      >
        <div className="card-header bg-primary text-white d-flex justify-content-between align-items-center">
          <h5 className="mb-0">
            <i className="fas fa-door-open me-2"></i>
            {room.name}
          </h5>
          <button className="btn btn-sm btn-outline-light" onClick={onClose}>
            <i className="fas fa-times"></i>
          </button>
        </div>

        <div className="card-body overflow-auto">
          <div className="row">
            {/* Left column — images */}
            <div className="col-md-6 mb-3">
              <div className="position-relative">
                {images.length > 0 ? (
                  <>
                    <img
                      src={images[currentImageIndex]}
                      alt={`Room ${currentImageIndex + 1}`}
                      className="img-fluid rounded"
                      style={{ width: '100%', height: '250px', objectFit: 'cover' }}
                    />
                    {images.length > 1 && (
                      <>
                        <button
                          className="btn btn-dark btn-sm position-absolute top-50 start-0 translate-middle-y ms-2"
                          onClick={prevImage}
                        >
                          <i className="fas fa-chevron-left"></i>
                        </button>
                        <button
                          className="btn btn-dark btn-sm position-absolute top-50 end-0 translate-middle-y me-2"
                          onClick={nextImage}
                        >
                          <i className="fas fa-chevron-right"></i>
                        </button>
                      </>
                    )}
                    <div className="position-absolute bottom-0 end-0 me-2 mb-2">
                      <span className="badge bg-dark">
                        {currentImageIndex + 1} / {images.length}
                      </span>
                    </div>
                  </>
                ) : (
                  <div
                    className="bg-light rounded d-flex align-items-center justify-content-center text-muted"
                    style={{ height: '250px' }}
                  >
                    <i className="fas fa-image fa-3x"></i>
                  </div>
                )}
              </div>
            </div>

            {/* Right column — info */}
            <div className="col-md-6">
              <div className="mb-3">
                <h6 className="text-muted">Description</h6>
                <p className="mb-2">{room.description}</p>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Price</h6>
                <div className="d-flex align-items-center justify-content-center">
                  <span className="h4 text-success mb-0">{room.pricePerHour} €</span>
                  <span className="text-muted ms-2">/ hour</span>
                </div>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Status</h6>
                <span className={`badge ${room.status ? 'bg-success' : 'bg-danger'}`}>
                  <i className={`fas ${room.status ? 'fa-check' : 'fa-times'} me-1`}></i>
                  {room.status ? 'Active' : 'Inactive'}
                </span>
              </div>
            </div>
          </div>

          {/* Address */}
          {room.address && (
            <div className="mb-3">
              <h6 className="text-muted">
                <i className="fas fa-map-marker-alt me-1"></i>
                Address
              </h6>
              <div className="bg-light p-3 rounded">
                <div className="row">
                  <div className="col-md-6">
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
                  <div className="col-md-6">
                    {room.address.phoneNumber && (
                      <p className="mb-1">
                        <i className="fas fa-phone me-1"></i>
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
              <h6 className="text-muted">
                <i className="fas fa-list me-1"></i>
                Available activities
              </h6>
              <div className="row">
                {room.activity.map((activity) => (
                  <div key={activity.activityId} className="col-md-6 mb-2">
                    <div className="card border-primary">
                      <div className="card-body p-3">
                        <h6 className="card-title mb-1">{activity.name}</h6>
                        <p className="card-text small text-muted mb-0">{activity.description}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          )}

          {/* Actions */}
          <div className="d-flex gap-2 mt-4">
            <button className="btn btn-primary">
              <i className="fas fa-calendar-plus me-1"></i>
              Book
            </button>
            <button className="btn btn-outline-primary">
              <i className="fas fa-heart me-1"></i>Add to favourites
            </button>
            <button className="btn btn-outline-secondary">
              <i className="fas fa-share me-1"></i>
              Share
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RoomDetailOverlay;
