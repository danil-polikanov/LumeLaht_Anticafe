import React, { useState } from 'react';
import { setPagination } from '@/entities/room/model';
import {
  useGetRoomsByFiltersQuery,
  useGetRoomByIdQuery,
} from '@/entities/room';
import { selectFilters, selectSorting, selectPagination } from '@/entities/room/model';
import { RoomFilters } from '@/widgets/room-filters/ui';
import { RoomSortingAndPagination } from '@/widgets/room-sortPaggination/ui';
import { RoomCard } from '@/widgets/room-card/ui';
import { PaginationComponent } from '@/widgets/pagginationButtons/ui';
import { RoomDetailOverlay } from '@/widgets/room-details/ui';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';

export const RoomList: React.FC = () => {
  const dispatch = useAppDispatch();
  const filters = useAppSelector(selectFilters);
  const sorting = useAppSelector(selectSorting);
  const pagination = useAppSelector(selectPagination);

  const [selectedRoomId, setSelectedRoomId] = useState<string | null>(null);

  // RTK Query: rooms by filters — auto-refetches when args change
  const {
    data: roomsData,
    isLoading,
    isFetching,
    error: roomsError,
  } = useGetRoomsByFiltersQuery({
    filters,
    sorting,
    currentPage: pagination.currentPage,
    pageSize: pagination.pageSize,
  });

  // RTK Query: room by id — only fetches when selectedRoomId is set
  const { data: selectedRoom } = useGetRoomByIdQuery(selectedRoomId!, {
    skip: !selectedRoomId,
  });

  // Sync pagination from server response back to Redux
  React.useEffect(() => {
    if (roomsData?.pagination) {
      dispatch(setPagination(roomsData.pagination));
    }
  }, [roomsData?.pagination, dispatch]);

  const rooms = roomsData?.items ?? [];
  const error = roomsError ? 'Failed to load rooms' : null;

  const handleRoomClick = (roomId: string) => {
    setSelectedRoomId(roomId);
  };

  const handleCloseDetail = () => {
    setSelectedRoomId(null);
  };

  if (isLoading && rooms.length === 0) {
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
          {isFetching && (
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
          {selectedRoom && (
            <RoomDetailOverlay room={selectedRoom} onClose={handleCloseDetail} />
          )}

          <PaginationComponent />
        </div>
      </div>
    </div>
  );
};

export default RoomList;
