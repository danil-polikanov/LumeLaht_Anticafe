import React from 'react';
import { NavLink } from 'react-router-dom';

export const Logo = () => {
  return (
    <NavLink className="flex" to={'/'}>
      <img
        className="w-[100px] h-[100px] md:w-[75px] md:h-[75px] rounded-full bg-[#9c9393] transition-all duration-300 ease-in-out cursor-pointer hover:opacity-70"
        src="/Test_Logo-removebg-preview.png"
        alt="LumeLaht Logo"
      />
    </NavLink>
  );
};

export default Logo;
