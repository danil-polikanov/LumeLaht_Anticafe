import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAppDispatch } from '@/shared/lib/hooks/useRedux';
import { setFilters } from '@/entities/room/model';
import styles from './Welcome.module.css';

const CITIES = ['Tallinn', 'Tartu', 'Pärnu', 'Narva'];
const DEFAULT_CITY = 'Tallinn';

export const Welcome = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const handleCityClick = (city: string) => {
    dispatch(setFilters({ city }));
    navigate('/rooms');
  };

  return (
    <section className={styles.hero} style={{ backgroundImage: "url('/Test_LumeLaht_Image.jpg')" }}>
      <div className={styles.overlay} />
      <div className={styles.content}>
        <div className={styles.locationBadge}>
          <i className={`fas fa-map-marker-alt ${styles.locationBadgeIcon}`}></i>
          Pikk tn 36, Old Tallinn
        </div>

        <h1 className={styles.title}>Tere tulemast!</h1>
        <p className={styles.subtitle}>
          Your creative space awaits — choose a city and start exploring
        </p>

        {/* City tags */}
        <div className={styles.cityRow}>
          {CITIES.map((city) =>
            city === DEFAULT_CITY ? (
              <button
                key={city}
                className={styles.cityPillActive}
                onClick={() => handleCityClick(city)}
              >
                <i className="fas fa-map-pin text-accent"></i>
                {city}
                <i className="fas fa-check text-accent text-xs"></i>
              </button>
            ) : (
              <button key={city} className={styles.cityPill} onClick={() => handleCityClick(city)}>
                <i className={`fas fa-map-pin ${styles.cityPillIcon}`}></i>
                {city}
              </button>
            ),
          )}
        </div>

        {/* Action buttons */}
        <div className={styles.actions}>
          <Link to="/rooms" className={styles.primaryBtn}>
            <i className="fas fa-door-open mr-2"></i>Browse Rooms
          </Link>
          <a href="/about" className={styles.secondaryBtn}>
            <i className="fas fa-info-circle mr-2"></i>About Us
          </a>
        </div>
      </div>
    </section>
  );
};

export default Welcome;
