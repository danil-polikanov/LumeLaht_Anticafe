import React, { useEffect } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import { Header } from '@/widgets/header/ui';
import { About } from '@/pages/about/ui/About';
import { Contact } from '@/widgets/contact-section/ui';

export const Layout = () => {
  const location = useLocation();
  const isHomePage = ['/', '/about', '/contacts'].includes(location.pathname);

  useEffect(() => {
    if (location.pathname === '/contacts') {
      const el = document.getElementById('contact-section');
      if (el) el.scrollIntoView({ behavior: 'smooth' });
    }
    if (location.pathname === '/about') {
      const el = document.getElementById('about-section');
      if (el) el.scrollIntoView({ behavior: 'smooth' });
    }
  }, [location.pathname]);

  return (
    <div className="relative min-h-screen flex flex-col">
      <Header />
      <main>
        <Outlet />
        {isHomePage && (
          <section id="about-section">
            <About />
          </section>
        )}
      </main>
      <section id="contact-section">
        <Contact />
      </section>
    </div>
  );
};

export default Layout;
