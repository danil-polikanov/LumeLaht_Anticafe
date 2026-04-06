import React, { useEffect, useRef, useState } from 'react';
import styles from './AboutImage.module.css';

export const AboutImage = () => {
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
      <div className={styles.imageContainer}>
        <img src="/AboutPhoto.jpg" alt="Inside LumeLaht Anticafe" className={styles.image} />
        <div className={styles.statsCard}>
          <div className={styles.statValue}>500+</div>
          <div className={styles.statLabel}>Happy guests</div>
        </div>
      </div>
    </div>
  );
};

export default AboutImage;
