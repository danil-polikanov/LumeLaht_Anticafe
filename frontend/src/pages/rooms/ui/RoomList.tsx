import React, { useEffect, useRef, useState } from 'react';
import toast from 'react-hot-toast';
import { setPagination } from '@/entities/room/model';
import { useGetRoomsByFiltersQuery, useGetRoomByIdQuery } from '@/entities/room';
import { selectFilters, selectSorting, selectPagination } from '@/entities/room/model';
import { RoomFilters } from '@/widgets/room-filters/ui';
import { RoomSortingAndPagination } from '@/widgets/room-sortPaggination/ui';
import { RoomCard } from '@/widgets/room-card/ui';
import { PaginationComponent } from '@/widgets/pagginationButtons/ui';
import { RoomDetailOverlay } from '@/widgets/room-details/ui';
import { RoomCardSkeletonGrid } from '@/shared/ui/RoomCardSkeleton';
import { PageHero } from '@/shared/ui/PageHero';
import { useAppDispatch, useAppSelector } from '@/shared/lib/hooks/useRedux';
import styles from './RoomList.module.css';

export const RoomList: React.FC = () => {
  const dispatch = useAppDispatch();
  const filters = useAppSelector(selectFilters);
  const sorting = useAppSelector(selectSorting);
  const pagination = useAppSelector(selectPagination);
  const prevErrorRef = useRef(false);

  const [selectedRoomId, setSelectedRoomId] = useState<string | null>(null);

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

  const { data: selectedRoom } = useGetRoomByIdQuery(selectedRoomId ?? '', {
    skip: !selectedRoomId,
  });

  useEffect(() => {
    if (roomsData?.pagination) {
      dispatch(setPagination(roomsData.pagination));
    }
  }, [roomsData?.pagination, dispatch]);

  useEffect(() => {
    if (roomsError && !prevErrorRef.current) {
      toast.error('Could not load rooms. Please check your connection.');
    }
    prevErrorRef.current = !!roomsError;
  }, [roomsError]);

  const rooms = roomsData?.items ?? [];

  return (
    <>
      <PageHero
        title="Room Catalog"
        subtitle="Find the perfect space for your next event, meeting, or creative session"
        breadcrumbs={[
          { label: 'Home', to: '/' },
          { label: 'Rooms' },
        ]}
      />
      <div className={styles.page}>
        {/* Filters (horizontal pills + search) */}
        <RoomFilters />

        {/* Sort row */}
        <div className={styles.headerRow}>
          <h2 className={styles.pageTitle}>
            <i className="fas fa-building mr-2 text-accent"></i>
            Room catalog
          </h2>
          <RoomSortingAndPagination />
        </div>

        {/* Loading indicator */}
        {isFetching && !isLoading && (
          <div className={styles.updatingIndicator}>
            <div className={styles.spinner} role="status" />
            <span className={styles.updatingText}>Updating...</span>
          </div>
        )}

        {/* Room grid (full-width) */}
        <div className={styles.roomGrid}>
          {isLoading ? (
            <RoomCardSkeletonGrid count={6} />
          ) : roomsError ? (
            <ErrorState />
          ) : rooms.length > 0 ? (
            rooms.map((room) => (
              <RoomCard
                key={room.roomId}
                room={room}
                onRoomClick={(id) => setSelectedRoomId(id)}
              />
            ))
          ) : (
            <EmptyState />
          )}
        </div>

        {selectedRoomId && selectedRoom && (
          <RoomDetailOverlay room={selectedRoom} onClose={() => setSelectedRoomId(null)} />
        )}

        {!isLoading && !roomsError && <PaginationComponent />}
      </div>
    </>
  );
};

const ErrorState: React.FC = () => (
  <div className={styles.stateContainer}>
    <div className={styles.stateInner}>
      <div className={`${styles.stateIcon} text-accent-300`}>
        <i className="fas fa-cloud-sun-rain text-6xl"></i>
      </div>
      <h4 className={styles.stateTitle}>Oops, something went wrong</h4>
      <p className={styles.stateText}>
        We couldn&apos;t load the rooms right now. This might be a temporary issue &mdash; try
        refreshing the page in a moment.
      </p>
      <p className="text-gray-300 text-sm mt-4">
        <i className="fas fa-sync-alt mr-1"></i>
        <a
          href="#"
          className={styles.refreshLink}
          onClick={(e) => {
            e.preventDefault();
            window.location.reload();
          }}
        >
          Refresh page
        </a>
      </p>
    </div>
  </div>
);

const EmptyState: React.FC = () => (
  <div className={styles.stateContainer}>
    <div className={styles.stateInner}>
      <div className={`${styles.stateIcon} text-accent-200`}>
        <i className="fas fa-couch text-6xl"></i>
      </div>
      <h4 className={styles.stateTitle}>Rooms are taking a nap...</h4>
      <p className={styles.stateText}>
        No rooms match your current filters. Try adjusting the filters or clearing them to see all
        available rooms.
      </p>
    </div>
  </div>
);

export default RoomList;
