import React from 'react';
import styles from './About.module.css';
const AboutImage = () => {
    return (
        <div className={styles.imageContainer}>
            <img src="/AboutPhoto.jpg" alt="Inside LumeLaht Anticafe" />
        </div>
    );
};
export default AboutImage;
