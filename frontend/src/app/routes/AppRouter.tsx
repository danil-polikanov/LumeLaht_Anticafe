import React from 'react';
import { Routes, Route } from 'react-router-dom';
import { Layout } from '@/app/layout/ui';
import { Home } from '@/pages/home/ui';
import { RoomList } from '@/pages/rooms/ui';
import { MyBookings } from '@/pages/my-bookings/ui/MyBookings';

export const AppRouter: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="about" element={<Home />} />
        <Route path="contacts" element={<Home />} />
        <Route path="rooms" element={<RoomList />} />
        <Route path="my-bookings" element={<MyBookings />} />
      </Route>
    </Routes>
  );
};
