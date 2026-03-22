import React, { useEffect, useRef, useState } from 'react';

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
    <div
      ref={ref}
      className={`flex-1 text-center px-3 transition-all duration-700 ${
        isVisible ? 'opacity-100 translate-x-0' : 'opacity-0 translate-x-12'
      }`}
    >
      <img src="/AboutPhoto.jpg" alt="Inside LumeLaht Anticafe" className="max-w-full rounded" />
    </div>
  );
};

export default AboutImage;
