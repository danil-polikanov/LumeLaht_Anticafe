import React from 'react';
import Header from './headerComponents/Header';
import Footer from './Footer';
import Home from './homeComponents/Home';
import Map from './homeComponents/Map';
import styles from './Layout.module.css';
type Props = {
    children: React.ReactNode;
};

export const Layout: React.FC<Props> = ({ children }) => (
    <div>
        <div className={styles.Layout_menu}>
            <Header />
            <Home />
        </div>
        <main>{children}</main>
        <Map />
        <Footer />
    </div>
);
