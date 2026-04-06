import React from 'react';
import { NavLink } from 'react-router-dom';

interface NavMenuProps {
  isMobile?: boolean;
  onItemClick?: () => void;
}

export const NavMenu: React.FC<NavMenuProps> = ({ isMobile = false, onItemClick }) => {
  const menuItems = [
    { label: 'Home', href: '/' },
    { label: 'About', href: '/about' },
    { label: 'Rooms', href: '/rooms' },
    { label: 'Contacts', href: '/contacts' },
  ];

  if (isMobile) {
    return (
      <nav className="w-full py-4">
        <ul className="flex flex-col gap-2 p-0 m-0 list-none">
          {menuItems.map((item, index) => (
            <li
              key={item.label}
              className="w-full opacity-0 -translate-y-5"
              style={{ animation: `slideInFromTop 0.3s ease ${index * 0.1 + 0.1}s forwards` }}
            >
              <NavLink
                to={item.href}
                end={item.href === '/'}
                onClick={onItemClick}
                className={({ isActive }) =>
                  `block px-6 py-4 font-medium text-lg rounded-lg transition-all duration-300 no-underline border-l-4 ${
                    isActive
                      ? 'bg-accent/15 text-accent border-l-accent font-semibold'
                      : 'text-gray-800 border-l-transparent hover:bg-accent/10 hover:text-accent hover:border-l-accent hover:translate-x-1'
                  }`
                }
              >
                {item.label}
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
    );
  }

  return (
    <nav className="w-full h-full flex items-center justify-center">
      <ul className="flex flex-row gap-8 px-8 py-2 m-0 list-none items-center justify-center w-full">
        {menuItems.map((item) => (
          <li
            key={item.label}
            className="cursor-pointer px-4 py-2 rounded-[25px] transition-all duration-300 font-medium relative hover:bg-white/10 hover:-translate-y-0.5"
          >
            <NavLink
              to={item.href}
              end={item.href === '/'}
              className={({ isActive }) =>
                `nav-link-animated text-white no-underline text-base font-medium transition-all duration-300 relative block ${
                  isActive ? 'font-semibold active' : ''
                }`
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
