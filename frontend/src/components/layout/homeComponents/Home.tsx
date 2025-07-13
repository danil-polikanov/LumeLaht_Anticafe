import React from 'react';
import styles from './Home.module.css';
const Home = () => {
    return (
        <div className={`container ${styles.home_welcome}`}>
            <h2> Tere tulemast!</h2>
            <p>Here you can order a room for your company</p>
        </div>
    );
};

export default Home;
