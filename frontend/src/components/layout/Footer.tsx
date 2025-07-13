import React from 'react';
import styles from './Footer.module.css';
import { NavLink, NavLinkProps } from 'react-router-dom';
import { IconType } from 'react-icons';
import {
    FaTelegram,
    FaInstagramSquare,
    FaLinkedin,
    FaFacebookMessenger,
} from 'react-icons/fa';

type IconsMenu = {
    tag: IconType;
    label: string;
    href: string;
};

const Footer = () => {
    const iconsMenu: IconsMenu[] = [
        { tag: FaTelegram, label: 'Telegram', href: '/telegram' },
        { tag: FaInstagramSquare, label: 'Instagram', href: '/instagram' },
        { tag: FaFacebookMessenger, label: 'Facebook', href: '/facebook' },
        { tag: FaLinkedin, label: 'LinkedIn', href: '/linkedIn' },
    ];

    return (
        <footer
            className={`container d-md-flex gap-4 align-items-center ${styles.footer_main}`}
        >
            <div>
                <h2>Address</h2>
                <p>Pikk tn 36, Old Tallinn,</p>
                <p>City Center, Tallin</p>
            </div>
            <div>
                <h2>Contact</h2>
                <p>Number:+54153215323245</p>
                <p>E-mail: Info@gmail.com</p>
            </div>
            <div>
                <h2>Opening hours</h2>
                <p>Mon-Sat: 11AM - 23PM</p>
                <p>Sunday: 11AM - 21PM</p>
            </div>
            <div>
                <h2>Follow Us</h2>
                <div className="d-flex justify-content-between">
                    {iconsMenu.map((icon, index) => {
                        const IconComponent = icon.tag;
                        return (
                            <a
                                key={index}
                                href={icon.href}
                                aria-label={icon.label}
                            >
                                {React.createElement(IconComponent, {
                                    size: 24,
                                })}
                            </a>
                        );
                    })}
                </div>
            </div>
        </footer>
    );
};
export default Footer;
