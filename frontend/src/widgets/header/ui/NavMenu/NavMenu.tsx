import React from 'react';
import { NavLink } from 'react-router-dom';
import styles from './NavMenu.module.css';

interface NavMenuProps {
  isMobile?: boolean;
}

export const NavMenu: React.FC<NavMenuProps> = ({ isMobile = false }) => {
  const menuItems = [
    { label: 'Home', href: '/' },
    { label: 'About', href: '/about' },
    { label: 'Book', href: '/book' },
    { label: 'Rooms', href: '/rooms' },
    { label: 'Contacts', href: '/contacts' },
  ];

  return (
    <nav className={isMobile ? styles.mobileNav : styles.desktopNav}>
      <ul className={isMobile ? styles.mobileNavList : styles.navList}>
        {menuItems.map((item) => (
          <li key={item.label} className={isMobile ? styles.mobileNavItem : styles.navItem}>
            <NavLink
              to={item.href}
              className={({ isActive }) =>
                `${
                  isMobile ? styles.mobileNavLink : styles.navLink
                } ${isActive ? styles.active : ''}`
              }
            >
              {item.label}
            </NavLink>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default NavMenu;
