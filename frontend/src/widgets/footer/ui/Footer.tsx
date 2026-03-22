import React from 'react';
import { IconType } from 'react-icons';
import {
  FaTelegram,
  FaInstagramSquare,
  FaLinkedin,
  FaFacebookMessenger,
  FaAddressBook,
} from 'react-icons/fa';
import { BsPhone, BsSearchHeart } from 'react-icons/bs';
import { LiaClockSolid } from 'react-icons/lia';

type IconsMenu = { tag: IconType; label: string; href: string };

export const Footer = () => {
  const iconsMenu: IconsMenu[] = [
    { tag: FaTelegram, label: 'Telegram', href: '/telegram' },
    { tag: FaInstagramSquare, label: 'Instagram', href: '/instagram' },
    { tag: FaFacebookMessenger, label: 'Facebook', href: '/facebook' },
    { tag: FaLinkedin, label: 'LinkedIn', href: '/linkedIn' },
  ];

  const socialColors: Record<string, string> = {
    telegram: 'before:bg-[#0088cc]',
    instagram: 'before:bg-gradient-to-br before:from-[#f09433] before:to-[#bc1888]',
    facebook: 'before:bg-gradient-to-br before:from-[#4267b2] before:to-[#898f9c]',
    linkedin: 'before:bg-gradient-to-br before:from-[#0077b5] before:to-[#00a0dc]',
  };

  return (
    <footer className="relative bg-gradient-to-br from-[#1a1a2e] via-[#16213e] to-[#0f3460] text-white overflow-hidden">
      {/* Decorative wave */}
      <div className="absolute top-0 left-0 w-full h-[60px] overflow-hidden leading-[0]">
        <svg
          viewBox="0 0 1200 120"
          preserveAspectRatio="none"
          className="relative block w-[calc(100%+1.3px)] h-[60px]"
        >
          <path
            d="M0,60 C200,20 400,40 600,30 C800,20 1000,40 1200,60 L1200,0 L0,0 Z"
            fill="#f8f9fa"
          />
        </svg>
      </div>

      <div className="container mx-auto max-w-6xl py-20 px-4">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          <h2 className="hidden"></h2>

          {/* Address */}
          <div className="footer-section p-6 bg-white/5 rounded-2xl border border-white/10 transition-all duration-300 hover:-translate-y-1 hover:shadow-[0_15px_35px_rgba(0,0,0,0.2)] hover:border-white/20">
            <div className="flex items-center mb-3">
              <div className="icon-box w-[50px] h-[50px] rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 hover:rotate-6 hover:shadow-[0_10px_25px_rgba(0,0,0,0.3)] bg-gradient-to-br from-[#667eea] to-[#764ba2] mr-3">
                <FaAddressBook size={24} />
              </div>
              <h3 className="text-xl font-semibold m-0 bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
                Address
              </h3>
            </div>
            <div>
              <div className="flex items-center mb-2">
                <i className="fas fa-map-marker-alt mr-2 w-6 text-center"></i>
                <span>Pikk tn 36, Old Tallinn</span>
              </div>
              <div className="flex items-center">
                <i className="fas fa-city mr-2 w-6 text-center"></i>
                <span>City Center, Tallin</span>
              </div>
            </div>
          </div>

          {/* Contacts */}
          <div className="footer-section p-6 bg-white/5 rounded-2xl border border-white/10 transition-all duration-300 hover:-translate-y-1 hover:shadow-[0_15px_35px_rgba(0,0,0,0.2)] hover:border-white/20">
            <div className="flex items-center mb-3">
              <div className="icon-box w-[50px] h-[50px] rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 hover:rotate-6 hover:shadow-[0_10px_25px_rgba(0,0,0,0.3)] bg-gradient-to-br from-[#f093fb] to-[#f5576c] mr-3">
                <BsPhone size={24} />
              </div>
              <h3 className="text-xl font-semibold m-0 bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
                Communication
              </h3>
            </div>
            <div>
              <a
                href="tel:+54153215323245"
                className="flex items-center mb-2 text-white no-underline hover:text-white"
              >
                <i className="fas fa-phone mr-2 w-6 text-center"></i>
                <span>+54153215323245</span>
              </a>
              <a
                href="mailto:Info@gmail.com"
                className="flex items-center text-white no-underline hover:text-white"
              >
                <i className="fas fa-envelope mr-2 w-6 text-center"></i>
                <span>Info@gmail.com</span>
              </a>
            </div>
          </div>

          {/* Working hours */}
          <div className="footer-section p-6 bg-white/5 rounded-2xl border border-white/10 transition-all duration-300 hover:-translate-y-1 hover:shadow-[0_15px_35px_rgba(0,0,0,0.2)] hover:border-white/20">
            <div className="flex items-center mb-3">
              <div className="icon-box w-[50px] h-[50px] rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 hover:rotate-6 hover:shadow-[0_10px_25px_rgba(0,0,0,0.3)] bg-gradient-to-br from-[#4facfe] to-[#00f2fe] mr-3">
                <LiaClockSolid size={24} />
              </div>
              <h3 className="text-xl font-semibold m-0 bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
                Working hours
              </h3>
            </div>
            <div>
              <div className="flex justify-between mb-2 text-white/80 text-[0.95rem] py-1 border-b border-white/10">
                <span>Mon-Sat:</span>
                <span className="font-semibold text-[#4facfe]">11:00 - 23:00</span>
              </div>
              <div className="flex justify-between text-white/80 text-[0.95rem] py-1 border-b border-white/10">
                <span>Sunday:</span>
                <span className="font-semibold text-[#4facfe]">11:00 - 21:00</span>
              </div>
            </div>
          </div>

          {/* Social media */}
          <div className="footer-section p-6 bg-white/5 rounded-2xl border border-white/10 transition-all duration-300 hover:-translate-y-1 hover:shadow-[0_15px_35px_rgba(0,0,0,0.2)] hover:border-white/20">
            <div className="flex items-center mb-3">
              <div className="icon-box w-[50px] h-[50px] rounded-xl flex items-center justify-center transition-all duration-300 hover:scale-110 hover:rotate-6 hover:shadow-[0_10px_25px_rgba(0,0,0,0.3)] bg-gradient-to-br from-[#43e97b] to-[#38f9d7] mr-3">
                <BsSearchHeart size={24} />
              </div>
              <h3 className="text-xl font-semibold m-0 bg-gradient-to-r from-white to-gray-300 bg-clip-text text-transparent">
                Follow us
              </h3>
            </div>
            <div className="flex gap-3">
              {iconsMenu.map((icon, index) => {
                const IconComponent = icon.tag;
                const colorClass = socialColors[icon.label.toLowerCase()] ?? '';
                return (
                  <a
                    key={index}
                    href={icon.href}
                    aria-label={icon.label}
                    className={`social-link w-[45px] h-[45px] rounded-xl flex items-center justify-center no-underline transition-all duration-300 bg-white/10 text-white hover:-translate-y-1 hover:scale-105 hover:shadow-[0_10px_25px_rgba(0,0,0,0.3)] ${colorClass}`}
                  >
                    <IconComponent size={24} />
                  </a>
                );
              })}
            </div>
          </div>
        </div>

        {/* Footer bottom */}
        <div className="border-t border-white/10 mt-10 pt-8 relative z-[2]">
          <div className="flex flex-col md:flex-row items-center justify-between gap-3">
            <div className="flex items-center">
              <div className="w-[30px] h-[30px] bg-gradient-to-br from-[#ff6b6b] to-[#ee5a24] rounded-full flex items-center justify-center mr-3 animate-heartbeat">
                <i className="fas fa-heart w-6 text-center"></i>
              </div>
              <span className="text-white/80 text-[0.95rem]">Made with love</span>
            </div>
            <span className="text-white/60 text-[0.9rem]">© 2026 All rights reserved</span>
          </div>
        </div>
      </div>

      {/* Animated elements */}
      <div className="absolute top-0 left-0 w-full h-full overflow-hidden pointer-events-none">
        <div className="absolute w-[80px] h-[80px] bg-gradient-to-br from-[#667eea] to-[#764ba2] rounded-full opacity-10 animate-float top-[20%] right-[10%]"></div>
        <div className="absolute w-[120px] h-[120px] bg-gradient-to-br from-[#f093fb] to-[#f5576c] rounded-full opacity-10 animate-float-2 bottom-[30%] left-[5%]"></div>
        <div className="absolute w-[60px] h-[60px] bg-gradient-to-br from-[#4facfe] to-[#00f2fe] rounded-full opacity-10 animate-float-4 top-[60%] right-[30%]"></div>
      </div>
    </footer>
  );
};

export default Footer;
