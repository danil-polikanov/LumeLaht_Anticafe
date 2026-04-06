import React from 'react';

export const RoomCardSkeleton: React.FC = () => {
  return (
    <div className="w-full md:w-1/2 lg:w-1/3 px-2 mb-4">
      <div className="bg-white rounded-xl border border-gray-100 h-full flex flex-col overflow-hidden">
        {/* Image skeleton */}
        <div className="skeleton h-[220px] rounded-none" />

        <div className="p-5 flex flex-col flex-1">
          {/* Title + status */}
          <div className="flex justify-between items-start mb-3">
            <div className="skeleton h-5 w-2/3 rounded-md" />
            <div className="skeleton h-6 w-16 rounded-full" />
          </div>

          {/* Description lines */}
          <div className="skeleton h-3 w-full rounded mb-2" />
          <div className="skeleton h-3 w-4/5 rounded mb-3" />

          {/* Address */}
          <div className="skeleton h-3 w-3/5 rounded mb-3" />

          {/* Activity badges */}
          <div className="flex gap-2 mb-3">
            <div className="skeleton h-6 w-16 rounded-full" />
            <div className="skeleton h-6 w-20 rounded-full" />
            <div className="skeleton h-6 w-14 rounded-full" />
          </div>

          {/* Price + button */}
          <div className="mt-auto pt-3 border-t border-gray-100 flex justify-between items-center">
            <div className="skeleton h-5 w-24 rounded" />
            <div className="skeleton h-9 w-28 rounded-lg" />
          </div>
        </div>
      </div>
    </div>
  );
};

export const RoomCardSkeletonGrid: React.FC<{ count?: number }> = ({ count = 6 }) => {
  return (
    <>
      {Array.from({ length: count }).map((_, i) => (
        <RoomCardSkeleton key={i} />
      ))}
    </>
  );
};
