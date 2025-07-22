// Footer.jsx
import React from 'react';
import styles from './Footer.module.css';
import { NavLink } from 'react-router-dom';
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
        <footer className={styles.footer}>
            {/* Декоративная волна */}
            <div className={styles.wave}>
                <svg viewBox="0 0 1200 120" preserveAspectRatio="none">
                    <path d="M985.66,92.83C906.67,72,823.78,31,743.84,14.19c-82.26-17.34-168.06-16.33-250.45.39C422.74,32.83,327.17,63.5,248.15,92.83C144.17,130.59,65.28,161.3,0,192.71V0H1200V95.8C1132.19,118.92,1055.71,111.31,985.66,92.83Z"></path>
                </svg>
            </div>

            <div className="container py-5">
                <div className="row g-4">
                    {/* Адрес */}
                    <div className="col-lg-3 col-md-6">
                        <div className={`${styles.section} h-100`}>
                            <div
                                className={`${styles.header} d-flex align-items-center mb-3`}
                            >
                                <div
                                    className={`${styles.iconBox} ${styles.addressIcon} me-3`}
                                >
                                    <FaAddressBook size={24} />
                                </div>
                                <h3 className={styles.sectionTitle}>Адрес</h3>
                            </div>
                            <div className={styles.content}>
                                <p className={`${styles.text} mb-2`}>
                                    <i className="fas fa-map-marker-alt me-2"></i>
                                    Pikk tn 36, Old Tallinn
                                </p>
                                <p className={`${styles.text} mb-0`}>
                                    <i className="fas fa-city me-2"></i>
                                    City Center, Tallin
                                </p>
                            </div>
                        </div>
                    </div>

                    {/* Контакты */}
                    <div className="col-lg-3 col-md-6">
                        <div className={`${styles.section} h-100`}>
                            <div
                                className={`${styles.header} d-flex align-items-center mb-3`}
                            >
                                <div
                                    className={`${styles.iconBox} ${styles.contactIcon} me-3`}
                                >
                                    <BsPhone size={24} />
                                </div>
                                <h3 className={styles.sectionTitle}>
                                    Контакты
                                </h3>
                            </div>
                            <div className={styles.content}>
                                <a
                                    href="tel:+54153215323245"
                                    className={`${styles.contactLink} d-block mb-2`}
                                >
                                    <i className="fas fa-phone me-2"></i>
                                    +54153215323245
                                </a>
                                <a
                                    href="mailto:Info@gmail.com"
                                    className={`${styles.contactLink} d-block`}
                                >
                                    <i className="fas fa-envelope me-2"></i>
                                    Info@gmail.com
                                </a>
                            </div>
                        </div>
                    </div>

                    {/* Время работы */}
                    <div className="col-lg-3 col-md-6">
                        <div className={`${styles.section} h-100`}>
                            <div
                                className={`${styles.header} d-flex align-items-center mb-3`}
                            >
                                <div
                                    className={`${styles.iconBox} ${styles.timeIcon} me-3`}
                                >
                                    <LiaClockSolid size={24} />
                                </div>
                                <h3 className={styles.sectionTitle}>
                                    Время работы
                                </h3>
                            </div>
                            <div className={styles.content}>
                                <div
                                    className={`${styles.timeSlot} d-flex justify-content-between mb-2`}
                                >
                                    <span>Пн-Сб:</span>
                                    <span className={styles.timeValue}>
                                        11:00 - 23:00
                                    </span>
                                </div>
                                <div
                                    className={`${styles.timeSlot} d-flex justify-content-between`}
                                >
                                    <span>Воскресенье:</span>
                                    <span className={styles.timeValue}>
                                        11:00 - 21:00
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Социальные сети */}
                    <div className="col-lg-3 col-md-6">
                        <div className={`${styles.section} h-100`}>
                            <div
                                className={`${styles.header} d-flex align-items-center mb-3`}
                            >
                                <div
                                    className={`${styles.iconBox} ${styles.socialIcon} me-3`}
                                >
                                    <BsSearchHeart size={24} />
                                </div>
                                <h3 className={styles.sectionTitle}>
                                    Следите за нами
                                </h3>
                            </div>
                            <div className={styles.content}>
                                <div className="d-flex gap-3">
                                    {iconsMenu.map((icon, index) => {
                                        const IconComponent = icon.tag;
                                        return (
                                            <a
                                                key={index}
                                                href={icon.href}
                                                aria-label={icon.label}
                                                className={`${
                                                    styles.socialLink
                                                } ${
                                                    styles[
                                                        icon.label.toLowerCase()
                                                    ]
                                                }`}
                                            >
                                                <IconComponent size={24} />
                                            </a>
                                        );
                                    })}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Нижняя часть */}
                <div className={`${styles.footerBottom} mt-5 pt-4`}>
                    <div className="row align-items-center">
                        <div className="col-md-6">
                            <div className="d-flex align-items-center">
                                <div className={styles.heartIcon}>
                                    <i className="fas fa-heart"></i>
                                </div>
                                <span className={styles.footerText}>
                                    Made with love
                                </span>
                            </div>
                        </div>
                        <div className="col-md-6 text-md-end mt-3 mt-md-0">
                            <span className={styles.copyright}>
                                © 2025 Все права защищены
                            </span>
                        </div>
                    </div>
                </div>
            </div>

            {/* Анимированные элементы */}
            <div className={styles.animatedBg}>
                <div
                    className={`${styles.floatingShape} ${styles.shape1}`}
                ></div>
                <div
                    className={`${styles.floatingShape} ${styles.shape2}`}
                ></div>
                <div
                    className={`${styles.floatingShape} ${styles.shape3}`}
                ></div>
            </div>
        </footer>
    );
};

export default Footer;
