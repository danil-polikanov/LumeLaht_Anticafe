import React from 'react';
import styles from './About.module.css';
import AboutContext from './AboutContext';
import AboutImage from './AboutImage';
const About = () => {
    return (
        <section id="about" className={styles.aboutSection}>
            <div className={styles.aboutContent}>
                <AboutContext />
                <AboutImage />
            </div>
        </section>
    );
};
export default About;
