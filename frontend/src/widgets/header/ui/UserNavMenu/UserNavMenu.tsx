import React from 'react';

interface UserNavMenuProps {
  isScrolled?: boolean;
}

export const UserNavMenu: React.FC<UserNavMenuProps> = ({ isScrolled = false }) => {
  const btnStyle = isScrolled
    ? 'text-accent-800 bg-accent-100 hover:bg-accent-200'
    : 'text-white bg-accent hover:bg-accent-hover';

  return (
    <ul className="flex flex-row gap-3 my-auto list-none p-0 m-0">
      <li>
        <button
          type="button"
          className={`user-btn-animated cursor-pointer px-4 py-2 transition-all duration-300 font-medium relative rounded-full whitespace-nowrap hover:-translate-y-0.5 border-none ${btnStyle}`}
        >
          Login
        </button>
      </li>
      <li>
        <button
          type="button"
          className={`user-btn-animated cursor-pointer px-4 py-2 transition-all duration-300 font-medium relative rounded-full whitespace-nowrap hover:-translate-y-0.5 border-none ${btnStyle}`}
        >
          Sign in
        </button>
      </li>
    </ul>
  );
};

export default UserNavMenu;
