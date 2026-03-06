import React, { useEffect } from 'react';
import { Location, Outlet, useLocation } from 'react-router-dom';
import styles from './Layout.module.css';
import { scroller } from 'react-scroll';
import { Header } from '@/widgets/header/ui';
import { About } from '@/pages/about/ui/About';
import { Contact } from '@/widgets/contact-section/ui';
export const Layout = () => {
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
        <Outlet />
        {isHomePage && <About />}
      </main>
      <section id="contact-section">
        <Contact />
      </section>
    </div>
  );
};

export default Layout;
