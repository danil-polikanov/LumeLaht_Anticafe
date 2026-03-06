import React from 'react';
import Map from './Map';
import { Footer } from '@/widgets/footer/ui';
import { useLocation } from 'react-router-dom';
export const Contact = () => {
  const location = useLocation();
  const isHomePage = ['/', '/about', '/contacts'].includes(location.pathname);
  return (
    <section id="contacts">
      {isHomePage && <Map />}
      <Footer></Footer>
    </section>
  );
};

export default Contact;
