import React from 'react';

export const UserNavMenu: React.FC = () => {
  return (
    <ul className="flex flex-row gap-3 my-auto list-none p-0 m-0">
      <li>
        <button
          type="button"
          className="user-btn-animated cursor-pointer px-3 py-1.5 transition-all duration-300 font-medium relative text-white bg-accent rounded-full whitespace-nowrap hover:-translate-y-0.5 border-none"
        >
          Login
        </button>
      </li>
      <li>
        <button
          type="button"
          className="user-btn-animated cursor-pointer px-3 py-1.5 transition-all duration-300 font-medium relative text-white bg-accent rounded-full whitespace-nowrap hover:-translate-y-0.5 border-none"
        >
          Sign in
        </button>
      </li>
    </ul>
  );
};

export default UserNavMenu;
