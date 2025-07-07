import React from 'react';
import styles from './Header.module.css';
import Logo from './Logo';
import NavMenu from './NavMenu';
import UserNavMenu from './userNavMenu/UserNavMenu';
const Header = () => {
    return (
        <header
            className={`container d-flex w-100 align-items-center flex-row ${styles.header}`}
        >
            <div className={styles.left}>
                <Logo />
            </div>
            <div className={styles.center}>
                <NavMenu />
            </div>
            <div className={styles.right}>
                <UserNavMenu />
            </div>
        </header>
    );
};

export default Header;
