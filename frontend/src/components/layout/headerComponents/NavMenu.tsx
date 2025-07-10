import React from 'react';
import { NavLink, NavLinkProps } from 'react-router-dom';
import styles from './NavMenu.module.css';

interface NavMenuProps {
    isMobile?: boolean;
}

const NavMenu: React.FC<NavMenuProps> = ({ isMobile = false }) => {
    const menuItems = [
        { label: 'Home', href: '/' },
        { label: 'About', href: '/about' },
        { label: 'Book', href: '/book' },
        { label: 'Rooms', href: '/rooms' },
    ];

    const getClassName =
        (baseClass: string) =>
        ({ isActive }: { isActive: boolean }): string =>
            `${baseClass} ${isActive ? styles.active : ''}`.trim();

    const renderMenu = (isMobileView: boolean) => (
        <nav className={isMobileView ? styles.mobileNav : styles.desktopNav}>
            <ul
                className={isMobileView ? styles.mobileNavList : styles.navList}
            >
                {menuItems.map((item) => (
                    <li
                        key={item.href}
                        className={
                            isMobileView ? styles.mobileNavItem : styles.navItem
                        }
                    >
                        <NavLink
                            to={item.href}
                            className={getClassName(
                                isMobileView
                                    ? styles.mobileNavLink
                                    : styles.navLink
                            )}
                        >
                            {item.label}
                        </NavLink>
                    </li>
                ))}
            </ul>
        </nav>
    );

    return renderMenu(isMobile);
};

export default NavMenu;

/*        <nav className="navbar navbar-expand-lg bg-body-tertiary">
            <ul className="d-flex flex-row gap-4 px-2 m-auto align-items-center list-unstyled">
                <li className={styles.navItem}>Home</li>
                <li className={styles.navItem}>About</li>
                <li className={styles.navItem}>Book</li>
                <li className={styles.navItem}>Rooms</li>
            </ul>
        </nav>
        */
