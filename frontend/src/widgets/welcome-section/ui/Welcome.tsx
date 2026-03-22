import React from 'react';

export const Welcome = () => {
  return (
    <div className="max-w-[800px] mx-auto px-4 bg-[color-mix(in_srgb,#2e2b28,transparent_95%)] backdrop-blur-[5px] rounded-[15px] p-8 flex flex-col items-center justify-center text-[#f9f9f9]">
      <h2>Tere tulemast!</h2>
      <p>Here you can order a room for your company</p>
    </div>
  );
};

export default Welcome;
