import React from 'react';
import styles from './About.module.css';

const AboutContext = () => {
    return (
        <div className={styles.contextContainer}>
            <h2 className="text-center">About</h2>
            <p>
                <strong>LumeLaht</strong> is a modern anti-cafe in the center of
                Tallinn, where guests pay for the time they spend, not for the
                drinks and snacks consumed.
            </p>
            <p>
                <strong>Concept:</strong> To create a comfortable space for
                work, study, meetings, and relaxation with unlimited drinks,
                snacks, and entertainment included in the price of time.
            </p>
            <strong>Main Services:</strong>
            <ul className="px-3">
                <li>
                    Hourly space rental (8–10 €/hour, then 6 €/hour, 25–30
                    €/day)
                </li>
                <li>Drinks and light snacks (depends on tariff)</li>
                <li>Includes: Wi-Fi, games, chargers, stationery</li>
            </ul>
            <strong>Additional Services:</strong>
            <ul className="px-3">
                <li>Event and workshop organization</li>
                <li>Private room rental for meetings</li>
                <li>
                    Subscriptions for regular visitors (discounts up to 20%)
                </li>
            </ul>
        </div>
    );
};

export default AboutContext;
