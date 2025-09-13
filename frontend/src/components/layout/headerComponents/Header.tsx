import React, { useEffect, useState } from 'react';
import styles from './Header.module.css';
import Logo from './Logo';
import NavMenu from './NavMenu';
import UserNavMenu from './userNavMenu/UserNavMenu';
import AnimatedBurger from './AnimatedBurger';
import Welcome from './welcomeComponents/Welcome';
import { useLocation } from 'react-router-dom';
const Header: React.FC = () => {
    const location = useLocation();
    const isHomePage = ['/', '/about', '/contacts'].includes(location.pathname);
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };
    // Close menu after resize lower 992
    useEffect(() => {
        const handleResize = () => {
            if (window.innerWidth >= 992) {
                setIsMenuOpen(false);
            }
        };
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);
    // Close menu after click not closest tag
    useEffect(() => {
        const handleClickOutside = (event: MouseEvent) => {
            const target = event.target as Element;
            if (isMenuOpen && !target.closest(`.${styles.header}`)) {
                setIsMenuOpen(false);
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () =>
            document.removeEventListener('mousedown', handleClickOutside);
    }, [isMenuOpen]);
    return (
        <div>
            <header
                className={`container d-flex gap-4 w-100 align-items-center flex-row ${styles.header}`}
            >
                <div className={styles.left}>
                    <Logo />
                </div>
                <div className={`d-lg-none ${styles.burgerContainer}`}>
                    <AnimatedBurger isOpen={isMenuOpen} onClick={toggleMenu} />
                </div>
                <div className={`d-none d-lg-flex ${styles.center}`}>
                    <NavMenu />
                </div>

                <div className={`d-none d-lg-flex ${styles.right}`}>
                    <UserNavMenu />
                </div>
                {/* Mobile */}
                {isMenuOpen && (
                    <div className={`d-lg-none ${styles.mobileMenu}`}>
                        <div className={styles.mobileMenuContent}>
                            <NavMenu isMobile={true} />
                            <div className={styles.mobileUserMenu}>
                                <UserNavMenu />
                            </div>
                        </div>
                    </div>
                )}
            </header>
            {isHomePage && <Welcome />}
        </div>
    );
};

export default Header;
