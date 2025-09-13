import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import './App.css';
import Layout from './components/layout/Layout';
import { AppRoutes } from './services/AppRoutes';

function App() {
    return (
        <BrowserRouter>
            <div className="App">
                <AppRoutes />
            </div>
        </BrowserRouter>
    );
}

export default App;
