import { Routes, Route } from 'react-router-dom';
import Home from '../components/layout/homeComponents/Home';
import GetRooms from '../roomComponents/GetRooms';

export const AppRoutes = () => (
    <Routes>
        <Route index element={<Home />} />
        <Route path="rooms" element={<GetRooms />} />
        {/* Добавь другие маршруты по мере необходимости */}
    </Routes>
);
