import React from 'react';
import styles from './AnimatedBurger.module.css';

interface AnimatedBurgerProps {
    isOpen: boolean;
    onClick: () => void;
    className?: string;
}

const AnimatedBurger: React.FC<AnimatedBurgerProps> = ({
    isOpen,
    onClick,
    className = '',
}) => {
    return (
        <button
            className={`navbar-toggler ${
                isOpen ? styles.active : ''
            } ${className}`}
            onClick={onClick}
            type="button"
            aria-label="Toggle navigation"
        >
            <div className={styles.burgerIcon}>
                <div className={styles.burgerLine}></div>
                <div className={styles.burgerLine}></div>
                <div className={styles.burgerLine}></div>
            </div>
        </button>
    );
};

export default AnimatedBurger;
