import React, { useEffect, useRef, useState } from 'react';
import styles from './AboutContext.module.css';

export const AboutContext = () => {
  const ref = useRef<HTMLDivElement>(null);
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    const observer = new IntersectionObserver(([entry]) => {
      if (entry.isIntersecting) setIsVisible(true);
    });
    if (ref.current) observer.observe(ref.current);
    return () => observer.disconnect();
  }, []);

  return (
    <div ref={ref} className={`${styles.wrapper} ${isVisible ? styles.visible : styles.hidden}`}>
      <span className={styles.badge}>
        <i className="fas fa-info-circle mr-1"></i>
        About us
      </span>

      <h2 className={styles.title}>
        Welcome to <span className={styles.titleAccent}>LumeLaht</span>
      </h2>

      <p className={styles.description}>
        A modern anti-cafe in the heart of Tallinn, where guests pay for time — not for drinks and
        snacks. We provide a comfortable space for work, study, meetings, and relaxation with
        unlimited beverages, snacks, and entertainment included.
      </p>

      <div className={styles.featureGrid}>
        <div className={styles.featureCard}>
          <div className={styles.featureIcon}>
            <i className="fas fa-wifi"></i>
          </div>
          <div>
            <div className={styles.featureTitle}>Free Wi-Fi</div>
            <div className={styles.featureText}>High-speed internet, chargers, and stationery</div>
          </div>
        </div>
        <div className={styles.featureCard}>
          <div className={styles.featureIcon}>
            <i className="fas fa-coffee"></i>
          </div>
          <div>
            <div className={styles.featureTitle}>Drinks & Snacks</div>
            <div className={styles.featureText}>
              Unlimited coffee, tea, and light snacks included
            </div>
          </div>
        </div>
        <div className={styles.featureCard}>
          <div className={styles.featureIcon}>
            <i className="fas fa-calendar-alt"></i>
          </div>
          <div>
            <div className={styles.featureTitle}>Events & Workshops</div>
            <div className={styles.featureText}>Host your events or join community workshops</div>
          </div>
        </div>
        <div className={styles.featureCard}>
          <div className={styles.featureIcon}>
            <i className="fas fa-gamepad"></i>
          </div>
          <div>
            <div className={styles.featureTitle}>Games & Fun</div>
            <div className={styles.featureText}>Board games, consoles, and more entertainment</div>
          </div>
        </div>
      </div>

      <div className={styles.pricingRow}>
        <span className={styles.pricingChip}>
          <i className="fas fa-clock"></i> 8-10 €/hour
        </span>
        <span className={styles.pricingChip}>
          <i className="fas fa-hourglass-half"></i> Then 6 €/hour
        </span>
        <span className={styles.pricingChip}>
          <i className="fas fa-sun"></i> 25 €/day
        </span>
      </div>
    </div>
  );
};

export default AboutContext;
