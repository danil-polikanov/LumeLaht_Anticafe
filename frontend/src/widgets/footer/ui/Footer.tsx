// Footer.tsx
import styles from './Footer.module.css';
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

export const Footer = () => {
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
                    <path d="M0,60 C200,20 400,40 600,30 C800,20 1000,40 1200,60 L1200,0 L0,0 Z"></path>
                </svg>
            </div>

            <div className="container py-5">
                <div className="row g-4">
                    <h2></h2>

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
                                <h3 className={styles.sectionTitle}>Adress</h3>
                            </div>
                            <div className={styles.content}>
                                <div className="d-flex align-items-center mb-2">
                                    <i
                                        className={`fas fa-map-marker-alt me-2 ${styles.icon}`}
                                    ></i>
                                    <span>Pikk tn 36, Old Tallinn</span>
                                </div>
                                <div className="d-flex align-items-center">
                                    <i
                                        className={`fas fa-city me-2 ${styles.icon}`}
                                    ></i>
                                    <span>City Center, Tallin</span>
                                </div>
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
                                    Communication
                                </h3>
                            </div>
                            <div className={styles.content}>
                                <a
                                    href="tel:+54153215323245"
                                    className="d-flex align-items-center mb-2 text-white text-decoration-none"
                                >
                                    <i
                                        className={`fas fa-phone me-2 ${styles.icon}`}
                                    ></i>
                                    <span>+54153215323245</span>
                                </a>
                                <a
                                    href="mailto:Info@gmail.com"
                                    className="d-flex align-items-center text-white text-decoration-none"
                                >
                                    <i
                                        className={`fas fa-envelope me-2 ${styles.icon}`}
                                    ></i>
                                    <span>Info@gmail.com</span>
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
                                    Working hours
                                </h3>
                            </div>
                            <div className={styles.content}>
                                <div
                                    className={`${styles.timeSlot} d-flex justify-content-between mb-2`}
                                >
                                    <span>Mon-Sat:</span>
                                    <span className={styles.timeValue}>
                                        11:00 - 23:00
                                    </span>
                                </div>
                                <div
                                    className={`${styles.timeSlot} d-flex justify-content-between`}
                                >
                                    <span>Sunday:</span>
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
                                    Follow us
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
                                    <i
                                        className={`fas fa-heart ${styles.icon}`}
                                    ></i>
                                </div>
                                <span className={styles.footerText}>
                                    Made with love
                                </span>
                            </div>
                        </div>
                        <div className="col-md-6 text-md-end mt-3 mt-md-0">
                            <span className={styles.copyright}>
                                Â© 2025 All rights reserved
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
