import React from 'react';
import styles from './About.module.css';
import { AboutContext, AboutImage } from '@/widgets/about-section/ui';
export const About = () => {
    return (
        <section id="about-section" className={styles.aboutSection}>
            <div className={styles.aboutContent}>
                <AboutContext />
                <AboutImage />
            </div>
        </section>
    );
};
export default About;
