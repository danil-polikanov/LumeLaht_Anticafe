import React, { useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { scroller } from 'react-scroll';
import styles from './Home.module.css';

const Home = () => {
    const location = useLocation();

    useEffect(() => {
        const params = new URLSearchParams(location.search);
        const section = params.get('scrollTo');
        if (section) {
            scroller.scrollTo(section, {
                smooth: true,
                duration: 500,
            });
        }
    }, [location.search]);
    return (
        <div className={`container ${styles.home_welcome}`}>
            <h2> Tere tulemast!</h2>
            <p>Here you can order a room for your company</p>
        </div>
    );
};

export default Home;
