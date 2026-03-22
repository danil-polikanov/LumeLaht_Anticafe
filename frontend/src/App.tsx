import React from 'react';
import { BrowserRouter } from 'react-router-dom';
import { ReduxProvider } from './app/providers/ReduxProvider';
import { AppRouter } from './app/routes/AppRouter';
import './app/styles/global.css';

export const App: React.FC = () => {
  return (
    <BrowserRouter>
      <ReduxProvider>
        <div className="App">
          <AppRouter />
        </div>
      </ReduxProvider>
    </BrowserRouter>
  );
};
export default App;
