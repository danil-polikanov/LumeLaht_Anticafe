import React, { useEffect } from 'react';
import { useSelector } from 'react-redux';
import { clearSelectedRoom } from '@/entities/room/model';
import { fetchRoomsByFilters, fetchActivities } from '@/entities/room/model';
import {
  selectRooms,
  selectSelectedRoom,
  selectLoading,
  selectError,
  selectFilters,
  selectSorting,
  selectPagination,
} from '@/entities/room/model';
import { fetchRoomById } from '@/entities/room/model';
import { RoomFilters } from '@/widgets/room-filters/ui';
import { RoomSortingAndPagination } from '@/widgets/room-sortPaggination/ui';
import { RoomCard } from '@/widgets/room-card/ui';
import { PaginationComponent } from '@/widgets/pagginationButtons/ui';
import { RoomDetailOverlay } from '@/widgets/room-details/ui';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';

export const RoomList: React.FC = () => {
  const dispatch = useAppDispatch();
  const rooms = useAppSelector(selectRooms);
  const filters = useAppSelector(selectFilters);
  const sorting = useAppSelector(selectSorting);
  const pagination = useAppSelector(selectPagination);
  const loading = useSelector(selectLoading);
  const error = useSelector(selectError);
  const selectedRoomDetail = useAppSelector(selectSelectedRoom);

  // Load activities on mount
  useEffect(() => {
    dispatch(fetchActivities());
  }, [dispatch]);

  useEffect(() => {
    dispatch(fetchRoomsByFilters());
  }, [dispatch, filters, sorting, pagination.currentPage, pagination.pageSize]);

  const handleRoomClick = (roomId: string) => {
    dispatch(fetchRoomById(roomId));
    console.log('Navigate to room detail:', roomId);
  };

  const handleCloseDetail = () => {
    dispatch(clearSelectedRoom());
  };

  if (loading && rooms.length === 0) {
    return (
      <div className="w-full py-4">
        <div className="flex justify-center items-center min-h-[400px]">
          <div className="text-center">
            <div
              className="w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full animate-spin mx-auto"
              role="status"
            >
              <span className="sr-only">Loading...</span>
            </div>
            <div className="mt-3 text-gray-500">Room loading...</div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="w-full py-4">
        <div
          className="bg-red-50 border border-red-200 text-red-700 p-4 rounded flex items-center"
          role="alert"
        >
          <i className="fas fa-exclamation-triangle mr-2"></i>
          <div>
            <strong>Loading error!</strong> {error}
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="w-full py-4">
      {/* Header */}
      <div className="flex flex-wrap mb-4">
        <div className="w-full">
          <div className="flex justify-between items-center">
            <h2 className="mb-0 text-black">
              <i className="fas fa-building mr-2 text-blue-600"></i>
              Room catalog
            </h2>
            {/* Sorting */}
            <div>
              <RoomSortingAndPagination />
            </div>
            <button
              className="bg-green-500 text-white px-3 py-1.5 rounded hover:bg-green-600 border-none cursor-pointer"
              onClick={() => console.log('Add new room')}
            >
              <i className="fas fa-plus mr-1"></i>
              Add Room
            </button>
          </div>
        </div>
      </div>

      {/* Filters + Room list */}
      <div className="flex flex-wrap">
        {/* Filters sidebar */}
        <div className="w-full lg:w-1/4 pr-0 lg:pr-4">
          <div className="mb-4">
            <RoomFilters />
          </div>
          {/* Loading indicator while filtering */}
          {loading && (
            <div className="mb-3">
              <div className="flex justify-center items-center">
                <div
                  className="w-4 h-4 border-2 border-blue-600 border-t-transparent rounded-full animate-spin mr-2"
                  role="status"
                >
                  <span className="sr-only">Loading...</span>
                </div>
                <span className="text-gray-500">Updating results...</span>
              </div>
            </div>
          )}
        </div>

        {/* Room list */}
        <div className="w-full lg:w-3/4">
          <div className="flex flex-wrap -mx-2">
            {rooms.length > 0 ? (
              rooms.map((room) => (
                <RoomCard key={room.roomId} room={room} onRoomClick={handleRoomClick} />
              ))
            ) : (
              <div className="w-full">
                <div className="text-center py-5">
                  <div className="text-gray-500">
                    <i className="fas fa-search fa-3x mb-3"></i>
                    <h4>Rooms can't be found</h4>
                    <p>Try changing the filter settings or clear all filters</p>
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Room detail overlay */}
          {selectedRoomDetail && (
            <RoomDetailOverlay
              room={selectedRoomDetail || selectedRoomDetail}
              onClose={handleCloseDetail}
            />
          )}

          <PaginationComponent></PaginationComponent>
        </div>
      </div>
    </div>
  );
};

export default RoomList;
