import React from 'react';
import styles from './Map.module.css';
export const Map = () => {
  return (
    <div
      style={{
        width: '100%',
      }}
    >
      <iframe
        src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d3314.9649557706807!2d24.750608764179116!3d59.435620107717654!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sru!2see!4v1752331698891!5m2!1sru!2see"
        width="100%"
        height="500"
        style={{ border: 0, display: 'block' }}
        allowFullScreen
        loading="lazy"
        referrerPolicy="no-referrer-when-downgrade"
      />
    </div>
  );
};
export default Map;
