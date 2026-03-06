import React from 'react';
import styles from './Logo.module.css';
import { NavLink } from 'react-router-dom';
export const Logo = () => {
  return (
    <NavLink className="d-flex" to={'/'}>
      <img className={styles.logo} src="/Test_Logo-removebg-preview.png"></img>
    </NavLink>
  );
};

export default Logo;
