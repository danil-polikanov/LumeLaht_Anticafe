import React from 'react';
import styles from './UserNavMenu.module.css';
export const UserNavMenu: React.FC = () => {
    return (
        <ul className="d-flex flex-row gap-3 my-auto list-unstyled">
            <li className={styles.navItem}>
                <button type="button" className={`${styles.userButtons}`}>
                    Login
                </button>
            </li>
            <li className={styles.navItem}>
                <button type="button" className={`${styles.userButtons}`}>
                    Sign in
                </button>
            </li>
        </ul>
    );
};

export default UserNavMenu;
