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
        containerStyle={{ zIndex: 999999 }}
        toastOptions={{
          duration: 6000,
          style: {
            borderRadius: '14px',
            background: '#1f2937',
            color: '#fff',
            fontSize: '15px',
            padding: '14px 18px',
            minWidth: '320px',
            maxWidth: '480px',
            boxShadow: '0 10px 40px rgba(0,0,0,0.35)',
          },
          success: {
            iconTheme: { primary: '#CE9857', secondary: '#fff' },
          },
          error: {
            duration: 7000,
            iconTheme: { primary: '#ef4444', secondary: '#fff' },
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
