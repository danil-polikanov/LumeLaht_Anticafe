import React, { useEffect } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
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
      <Toaster
        position="top-right"
        toastOptions={{
          duration: 4000,
          style: {
            borderRadius: '12px',
            background: '#333',
            color: '#fff',
            fontSize: '14px',
          },
          success: {
            iconTheme: { primary: '#CE9857', secondary: '#fff' },
          },
        }}
      />
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
