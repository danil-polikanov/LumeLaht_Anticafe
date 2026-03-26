import React, { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAppSelector, useAppDispatch } from '@/shared/lib/hooks/useRedux';
import { logout } from '@/entities/auth';
import { AuthModal } from '@/widgets/auth-modal/ui/AuthModal';

interface UserNavMenuProps {
  isScrolled?: boolean;
}

export const UserNavMenu: React.FC<UserNavMenuProps> = ({ isScrolled = false }) => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { token, email } = useAppSelector((state) => state.auth);
  const [showAuthModal, setShowAuthModal] = useState(false);
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const btnStyle = isScrolled
    ? 'text-accent-800 bg-accent-100 hover:bg-accent-200'
    : 'text-white bg-accent hover:bg-accent-hover';

  const btnBase =
    'user-btn-animated cursor-pointer px-4 py-2 transition-all duration-300 font-medium relative rounded-full whitespace-nowrap hover:-translate-y-0.5 border-none';

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(e.target as Node)) {
        setShowDropdown(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const initials = email ? email.charAt(0).toUpperCase() : '?';

  if (token) {
    return (
      <>
        <div className="relative" ref={dropdownRef}>
          <button
            type="button"
            onClick={() => setShowDropdown((prev) => !prev)}
            className={`flex items-center gap-2 cursor-pointer px-2 py-1.5 rounded-full transition-all duration-300 border-none hover:-translate-y-0.5 ${
              isScrolled ? 'hover:bg-gray-100' : 'hover:bg-white/10'
            }`}
          >
            <div className="w-9 h-9 rounded-full bg-gradient-to-br from-accent to-accent-hover flex items-center justify-center text-white font-bold text-sm shadow-md">
              {initials}
            </div>
            <i className={`fas fa-chevron-down text-xs transition-transform ${showDropdown ? 'rotate-180' : ''} ${
              isScrolled ? 'text-gray-500' : 'text-white/70'
            }`}></i>
          </button>

          {showDropdown && (
            <div className="absolute right-0 top-full mt-2 w-56 bg-white rounded-xl shadow-xl border border-gray-100 py-2 z-[1100] animate-scale-in origin-top-right">
              <div className="px-4 py-3 border-b border-gray-100">
                <p className="text-sm font-semibold text-gray-800 truncate">{email}</p>
                <p className="text-xs text-gray-400">Logged in</p>
              </div>
              <button
                onClick={() => {
                  setShowDropdown(false);
                  navigate('/my-bookings');
                }}
                className="w-full text-left px-4 py-2.5 text-sm text-gray-700 hover:bg-accent-50 hover:text-accent transition-colors flex items-center gap-2.5 border-none bg-transparent cursor-pointer"
              >
                <i className="fas fa-calendar-check w-4 text-center text-gray-400"></i>
                My Bookings
              </button>
              <div className="border-t border-gray-100 mt-1 pt-1">
                <button
                  onClick={() => {
                    setShowDropdown(false);
                    dispatch(logout());
                    navigate('/');
                  }}
                  className="w-full text-left px-4 py-2.5 text-sm text-red-500 hover:bg-red-50 transition-colors flex items-center gap-2.5 border-none bg-transparent cursor-pointer"
                >
                  <i className="fas fa-sign-out-alt w-4 text-center"></i>
                  Logout
                </button>
              </div>
            </div>
          )}
        </div>
      </>
    );
  }

  return (
    <>
      <ul className="flex flex-row gap-3 my-auto list-none p-0 m-0">
        <li>
          <button
            type="button"
            onClick={() => setShowAuthModal(true)}
            className={`${btnBase} ${btnStyle}`}
          >
            Login
          </button>
        </li>
        <li>
          <button
            type="button"
            onClick={() => setShowAuthModal(true)}
            className={`${btnBase} ${btnStyle}`}
          >
            Sign Up
          </button>
        </li>
      </ul>
      {showAuthModal && <AuthModal onClose={() => setShowAuthModal(false)} />}
    </>
  );
};

export default UserNavMenu;
