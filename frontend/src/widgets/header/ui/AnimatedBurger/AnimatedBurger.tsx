import React from 'react';

interface AnimatedBurgerProps {
  isOpen: boolean;
  onClick: () => void;
  isScrolled?: boolean;
}

export const AnimatedBurger: React.FC<AnimatedBurgerProps> = ({
  isOpen,
  onClick,
  isScrolled = false,
}) => {
  const lineColor = isScrolled ? 'bg-accent-800' : 'bg-white';

  return (
    <button
      onClick={onClick}
      className="border-none outline-none relative w-10 h-10 p-0 flex items-center justify-center bg-transparent rounded cursor-pointer transition-all duration-300 hover:bg-black/5 focus:shadow-none"
      aria-label="Toggle menu"
    >
      <div className="relative w-6 h-[18px] flex flex-col justify-between cursor-pointer">
        <span
          className={`w-full h-[3px] ${lineColor} rounded-sm transition-all duration-300 origin-center ${
            isOpen ? 'rotate-45 translate-x-1 translate-y-[7px]' : ''
          }`}
        />
        <span
          className={`w-full h-[3px] ${lineColor} rounded-sm transition-all duration-300 ${
            isOpen ? 'opacity-0' : ''
          }`}
        />
        <span
          className={`w-full h-[3px] ${lineColor} rounded-sm transition-all duration-300 origin-center ${
            isOpen ? '-rotate-45 translate-x-[5px] -translate-y-[7px]' : ''
          }`}
        />
      </div>
    </button>
  );
};

export default AnimatedBurger;
