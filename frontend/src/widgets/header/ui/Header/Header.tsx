import React, { useEffect, useRef, useState } from 'react';
import { Logo } from '@/widgets/header/ui/Logo';
import { UserNavMenu } from '@/widgets/header/ui/UserNavMenu';
import { AnimatedBurger } from '@/widgets/header/ui/AnimatedBurger';
import { Welcome } from '@/widgets/welcome-section/ui/Welcome';
import { NavMenu } from '@/widgets/header/ui/NavMenu';
import { useLocation } from 'react-router-dom';

export const Header: React.FC = () => {
  const location = useLocation();
  const isHomePage = ['/', '/about', '/contacts'].includes(location.pathname);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const headerRef = useRef<HTMLElement>(null);

  const toggleMenu = () => setIsMenuOpen((prev) => !prev);
  const closeMenu = () => setIsMenuOpen(false);

  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth >= 992) setIsMenuOpen(false);
    };
    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (isMenuOpen && headerRef.current && !headerRef.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [isMenuOpen]);

  return (
    <div>
      <header
        ref={headerRef}
        className="fixed top-0 left-0 right-0 z-[1000] flex items-center pt-10 px-5 max-w-[1200px] mx-auto"
      >
        {/* Left: Logo */}
        <div className="flex items-center flex-1 min-w-0">
          <Logo />
        </div>

        {/* Burger (mobile only) */}
        <div className="relative z-[1001] ml-auto flex lg:hidden">
          <AnimatedBurger isOpen={isMenuOpen} onClick={toggleMenu} />
        </div>

        {/* Center: Nav (desktop only) */}
        <div className="hidden lg:flex flex-[0_0_auto] absolute left-1/2 -translate-x-1/2 bg-[#CE9857] rounded-[80px] px-5 min-w-[280px] max-w-[600px] justify-center items-center">
          <NavMenu />
        </div>

        {/* Right: User actions (desktop only) */}
        <div className="hidden lg:flex flex-1 justify-end items-center min-w-0 whitespace-nowrap">
          <UserNavMenu />
        </div>

        {/* Mobile menu dropdown */}
        {isMenuOpen && (
          <div className="lg:hidden absolute top-full left-0 right-0 bg-white rounded-b-[20px] shadow-[0_4px_20px_rgba(0,0,0,0.1)] p-5 animate-slide-down overflow-y-auto max-h-[80vh]">
            <div className="flex flex-col gap-5">
              <NavMenu isMobile={true} onItemClick={closeMenu} />
              <div className="border-t border-gray-200 pt-5">
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
