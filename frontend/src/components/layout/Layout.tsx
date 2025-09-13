import React, { useEffect } from 'react';
import Header from './headerComponents/Header';
import Footer from './headerComponents/contactComponents/Footer';
import { Location, Outlet, useLocation } from 'react-router-dom';
import Home from './homeComponents/Home';
import Map from './headerComponents/contactComponents/Map';
import styles from './Layout.module.css';
import Contact from './headerComponents/contactComponents/Contact';
import { scroller } from 'react-scroll';
import Welcome from './headerComponents/welcomeComponents/Welcome';
import About from './homeComponents/aboutComponents/About';
const Layout = () => {
    const location = useLocation();
    const isHomePage = ['/', '/about', '/contacts'].includes(location.pathname);

    useEffect(() => {
        if (location.pathname === '/contacts') {
            scroller.scrollTo('contact-section', {
                smooth: true,
                duration: 500,
            });
        }
        if (location.pathname === '/about') {
            scroller.scrollTo('about-section', {
                smooth: true,
                duration: 500,
            });
        }
    }, [location]);

    return (
        <div>
            <div className={styles.Layout_menu}>
                <Header />
            </div>
            <main>
                {isHomePage && <About></About>}
                <Outlet />
            </main>
            <section id="contact-section">
                <Contact />
            </section>
        </div>
    );
};

export default Layout;
