'use client';

import React from 'react';

interface ButtonProps {
  variant?: 'primary' | 'secondary' | 'header';
  label: string;
  onClick: () => void;
}

export default function Button({ variant = 'primary', label, onClick }: ButtonProps): JSX.Element {
  const baseStyles = '';
  const variantStyles = {
    primary: 'bg-primary text-white hover:bg-blue-700',
    secondary: 'bg-secondary text-white hover:bg-green-700',
    header: "inline-block rounded border-2 border-primary px-6 pb-[6px] pt-2 text-xs font-medium uppercase leading-normal text-primary transition duration-150 ease-in-out hover:border-primary-accent-300 hover:bg-primary-50/50 hover:text-primary-accent-300 focus:border-primary-600 focus:bg-primary-50/50 focus:text-primary-600 focus:outline-none focus:ring-0 active:border-primary-700 active:text-primary-700 motion-reduce:transition-none dark:text-primary-500 dark:hover:bg-blue-950 dark:focus:bg-blue-950"  
  };

  return (
    <button className={`${baseStyles} ${variantStyles[variant]}`} onClick={onClick}>
      {label}
    </button>
  );
}
