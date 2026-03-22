import React from 'react';
import { AboutContext } from '@/widgets/about-section/ui/AboutContext';
import { AboutImage } from '@/widgets/about-section/ui/AboutImage';

export const About = () => {
  return (
    <div className="bg-gradient-to-br from-[#f8f9fa] to-[#e9ecef]">
      <section className="font-['Inter'] py-16 px-8 bg-[#f9f9f9]">
        <div className="flex justify-between text-left gap-16 flex-wrap max-w-[1200px] mx-auto">
          <AboutContext />
          <AboutImage />
        </div>
      </section>
    </div>
  );
};

export default About;
