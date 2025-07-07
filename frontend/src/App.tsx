import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import './App.css';
import RoomList from './roomComponents/RoomList';
import { Layout } from './components/layout/Layout';
import { AppRoutes } from './services/AppRoutes';

function App() {
    return (
        <BrowserRouter>
            <div className="App">
                <Layout>
                    <AppRoutes />
                </Layout>
            </div>
        </BrowserRouter>
    );
}

export default App;
