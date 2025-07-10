import React from 'react';
import Header from './headerComponents/Header';
import Footer from './Footer';
import Main from './mainComponents/Main';
import styles from './Layout.module.css';
type Props = {
    children: React.ReactNode;
};

export const Layout: React.FC<Props> = ({ children }) => (
    <div>
        <div className={styles.Layout_menu}>
            <Header />
            <Main />
        </div>
        <main style={{ paddingTop: '50px' }}>{children}</main>
        <Footer />
    </div>
);
