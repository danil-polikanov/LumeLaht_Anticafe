import React from 'react';
import styles from './Main.module.css';
const Main = () => {
    return (
        <div className={styles.main_start}>
            <div className={`container ${styles.welcome_main}`}>
                <h2> Tere tulemast!</h2>
                <p>
                    Lorem ipsum dolor, sit amet consectetur adipisicing elit.
                    Laboriosam ex eveniet mollitia. Dolores unde, debitis
                    voluptatum perspiciatis ea vitae recusandae, quo, deserunt
                    eius vero iste sint! Excepturi id dignissimos cumque.
                </p>
            </div>
        </div>
    );
};

export default Main;
