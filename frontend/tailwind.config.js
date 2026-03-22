/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{js,jsx,ts,tsx}'],
  theme: {
    extend: {
      colors: {
        accent: {
          DEFAULT: '#CE9857',
          hover: '#b8863f',
        },
        primary: '#0d6efd',
      },
      keyframes: {
        slideDown: {
          from: { opacity: '0', transform: 'translateY(-10px)' },
          to: { opacity: '1', transform: 'translateY(0)' },
        },
        slideInFromTop: {
          from: { opacity: '0', transform: 'translateY(-20px)' },
          to: { opacity: '1', transform: 'translateY(0)' },
        },
        heartbeat: {
          '0%, 100%': { transform: 'scale(1)' },
          '50%': { transform: 'scale(1.1)' },
        },
        float: {
          '0%, 100%': { transform: 'translateY(0px) rotate(0deg)' },
          '50%': { transform: 'translateY(-20px) rotate(180deg)' },
        },
        fadeInLeft: {
          from: { opacity: '0', transform: 'translateX(-50px)' },
          to: { opacity: '1', transform: 'translateX(0)' },
        },
        fadeInRight: {
          from: { opacity: '0', transform: 'translateX(50px)' },
          to: { opacity: '1', transform: 'translateX(0)' },
        },
        burgerLineMiddle: {
          to: { opacity: '0' },
        },
        burgerLineTop: {
          to: { transform: 'rotate(45deg) translate(4px, 4px)' },
        },
        burgerLineBottom: {
          to: { transform: 'rotate(-45deg) translate(7px, -6px)' },
        },
      },
      animation: {
        'slide-down': 'slideDown 0.3s ease',
        'slide-in-from-top-1': 'slideInFromTop 0.3s ease 0.1s forwards',
        'slide-in-from-top-2': 'slideInFromTop 0.3s ease 0.2s forwards',
        'slide-in-from-top-3': 'slideInFromTop 0.3s ease 0.3s forwards',
        'slide-in-from-top-4': 'slideInFromTop 0.3s ease 0.4s forwards',
        heartbeat: 'heartbeat 2s ease-in-out infinite',
        float: 'float 6s ease-in-out infinite',
        'float-2': 'float 6s ease-in-out 2s infinite',
        'float-4': 'float 6s ease-in-out 4s infinite',
        'fade-in-left': 'fadeInLeft 0.8s ease-out 0.2s forwards',
        'fade-in-right': 'fadeInRight 0.8s ease-out 0.4s forwards',
      },
    },
  },
  plugins: [],
};
