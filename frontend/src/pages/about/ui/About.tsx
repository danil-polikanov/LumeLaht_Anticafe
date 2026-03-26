import React from 'react';
import { AboutContext } from '@/widgets/about-section/ui/AboutContext';
import { AboutImage } from '@/widgets/about-section/ui/AboutImage';

export const About = () => {
  return (
    <section className="bg-gradient-to-br from-gray-50 to-gray-100 py-20 px-6">
      <div className="flex justify-between items-center gap-12 flex-wrap max-w-[1200px] mx-auto">
        <AboutContext />
        <AboutImage />
      </div>
    </section>
  );
};

export default About;
