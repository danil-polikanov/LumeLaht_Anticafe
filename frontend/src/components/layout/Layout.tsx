import React from 'react';
import Header from './headerComponents/Header';
import Footer from './Footer';

type Props = {
    children: React.ReactNode;
};

export const Layout: React.FC<Props> = ({ children }) => (
    <div>
        <Header />
        <main>{children}</main>
        <Footer />
    </div>
);
