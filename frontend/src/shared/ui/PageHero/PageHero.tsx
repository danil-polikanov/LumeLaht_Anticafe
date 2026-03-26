import React from 'react';
import { Link } from 'react-router-dom';
import styles from './PageHero.module.css';

interface Breadcrumb {
  label: string;
  to?: string;
}

interface PageHeroProps {
  title: string;
  subtitle?: string;
  breadcrumbs?: Breadcrumb[];
  backgroundImage?: string;
}

export const PageHero: React.FC<PageHeroProps> = ({
  title,
  subtitle,
  breadcrumbs,
  backgroundImage = '/Test_LumeLaht_Image.jpg',
}) => {
  return (
    <section className={styles.banner} style={{ backgroundImage: `url('${backgroundImage}')` }}>
      <div className={styles.overlay} />
      <div className={styles.content}>
        <h1 className={styles.title}>{title}</h1>
        {subtitle && <p className={styles.subtitle}>{subtitle}</p>}
        {breadcrumbs && breadcrumbs.length > 0 && (
          <nav className={styles.breadcrumbs}>
            {breadcrumbs.map((crumb, idx) => (
              <React.Fragment key={idx}>
                {idx > 0 && <span className={styles.breadcrumbSep}>/</span>}
                {crumb.to ? (
                  <Link to={crumb.to} className={styles.breadcrumbLink}>
                    {crumb.label}
                  </Link>
                ) : (
                  <span className={styles.breadcrumbCurrent}>{crumb.label}</span>
                )}
              </React.Fragment>
            ))}
          </nav>
        )}
      </div>
    </section>
  );
};

export default PageHero;
