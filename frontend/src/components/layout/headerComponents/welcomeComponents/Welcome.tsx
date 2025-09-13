import React from 'react';
import styles from './Welcome.module.css';
const Welcome = () => {
    return (
        <div className={`container ${styles.home_welcome}`}>
            <h2>Tere tulemast!</h2>
            <p>Here you can order a room for your company</p>
        </div>
    );
};

export default Welcome;
