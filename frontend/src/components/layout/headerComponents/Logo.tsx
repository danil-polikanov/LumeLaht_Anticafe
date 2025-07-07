import React from 'react';
import styles from './Logo.module.css';
const Logo = () => {
    return (
        <div className="d-flex">
            <img className={styles.logo} src="/logo.jpg"></img>
        </div>
    );
};

export default Logo;
