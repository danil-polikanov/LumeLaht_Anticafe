import React from 'react';
import Header from './headerComponents/Header';
import Footer from './Footer';
import { useLocation } from 'react-router-dom';
import Home from './homeComponents/Home';
import Map from './homeComponents/Map';
import styles from './Layout.module.css';
type Props = {
    children: React.ReactNode;
};

export const Layout: React.FC<Props> = ({ children }) => {
    const location = useLocation();
    const isHomePage = location.pathname === '/';

    return (
        <div>
            <div className={styles.Layout_menu}>
                <Header />
                {isHomePage && <Home />}
            </div>
            <main>{children}</main>
            {isHomePage && <Map />}
            <Footer />
        </div>
    );
};
