import React from 'react';
import Map from './Map';
import Footer from './Footer';
import { useLocation } from 'react-router-dom';
const Contact = () => {
    const location = useLocation();
    const isHomePage = location.pathname === '/';
    return (
        <section id="contacts">
            {isHomePage && <Map />}
            <Footer></Footer>
        </section>
    );
};

export default Contact;
