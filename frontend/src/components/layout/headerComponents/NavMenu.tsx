import React from 'react';
import PropTypes from 'prop-types';
import styles from './NavMenu.module.css';
const NavMenu = () => {
    return (
        <nav>
            <ul className="d-flex flex-row px-5 gap-4 m-auto align-items-center list-unstyled">
                <li className={styles.navItem}>Home</li>
                <li className={styles.navItem}>About</li>
                <li className={styles.navItem}>Book</li>
                <li className={styles.navItem}>Rooms</li>
            </ul>
        </nav>
    );
};

export default NavMenu;
