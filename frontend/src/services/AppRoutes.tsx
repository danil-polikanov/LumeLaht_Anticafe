import { Routes, Route } from 'react-router-dom';
import About from '../components/layout/homeComponents/aboutComponents/About';
import RoomList from '../roomComponents/RoomList';
import Header from '../components/layout/headerComponents/Header';
import Home from '../components/layout/homeComponents/Home';
import RoomsList from '../roomComponents/RoomList';
import Layout from '../components/layout/Layout';
import TestRooms from '../roomComponents/TestRooms';

export const AppRoutes = () => (
    <Routes>
        <Route path="/" element={<Layout />}>
            <Route index element={<Home />} />
            <Route path="about" element={<Home />} />
            <Route path="contacts" element={<Home />} />
            <Route path="rooms" element={<RoomsList />} />
            <Route path="test" element={<TestRooms />} />
        </Route>
    </Routes>
);
