import React, { useEffect, useRef, useState } from 'react';
import styles from 'src/pages/about/ui/About.module.css';
export const AboutImage = () => {
    const ref = useRef(null);
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        const observer = new IntersectionObserver(([entry]) => {
            if (entry.isIntersecting) setIsVisible(true);
        });
        if (ref.current) observer.observe(ref.current);
        return () => observer.disconnect();
    }, []);
    return (
        <div
            ref={ref}
            className={`${styles.imageContainer} ${
                isVisible ? styles.animate : ''
            }`}
        >
            <img src="/AboutPhoto.jpg" alt="Inside LumeLaht Anticafe" />
        </div>
    );
};
export default AboutImage;
