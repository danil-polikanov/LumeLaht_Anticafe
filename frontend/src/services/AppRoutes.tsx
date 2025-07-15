import { Routes, Route } from 'react-router-dom';
import About from '../components/layout/homeComponents/aboutComponents/About';
import RoomList from '../roomComponents/RoomList';
import Header from '../components/layout/headerComponents/Header';

export const AppRoutes = () => (
    <Routes>
        <Route index element={<About />} />
        <Route path="rooms" element={<RoomList />} />
        {/* Добавь другие маршруты по мере необходимости */}
    </Routes>
);
