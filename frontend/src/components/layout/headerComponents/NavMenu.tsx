import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import styles from './NavMenu.module.css';

interface NavMenuProps {
    isMobile?: boolean;
}

const NavMenu: React.FC<NavMenuProps> = ({ isMobile = false }) => {
    const navigate = useNavigate();

    const menuItems = [
        { label: 'Home', href: '/' },
        { label: 'About', href: 'about', scrollTo: true },
        { label: 'Book', href: '/book' },
        { label: 'Rooms', href: '/rooms' },
        { label: 'Contacts', href: 'contacts', scrollTo: true },
    ];

    const getClassName =
        (baseClass: string) =>
        ({ isActive }: { isActive: boolean }): string =>
            `${baseClass} ${isActive ? styles.active : ''}`.trim();

    const handleScrollClick = (section: string) => {
        navigate(`/?scrollTo=${section}`);
    };

    const renderMenu = (isMobileView: boolean) => (
        <nav className={isMobileView ? styles.mobileNav : styles.desktopNav}>
            <ul
                className={isMobileView ? styles.mobileNavList : styles.navList}
            >
                {menuItems.map((item) => (
                    <li
                        key={item.label}
                        className={
                            isMobileView ? styles.mobileNavItem : styles.navItem
                        }
                    >
                        {item.scrollTo ? (
                            <div
                                onClick={() => handleScrollClick(item.href)}
                                className={
                                    isMobileView
                                        ? styles.mobileNavLink
                                        : styles.navLink
                                }
                            >
                                {item.label}
                            </div>
                        ) : (
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
                        )}
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
