import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: "var(--primary-color)",
        secondary: "var(--secondary-color)",
        background: "var(--background-color)",
        white: "var(--white-color)",
        "primary-text": "var(--primary-text-color)",
        "secondary-text": "var(--secondary-text-color)",
        success: "var(--success-color)",
        warning: "var(--warning-color)",
        error: "var(--error-color)",
        foreground: "var(--foreground)",
      },
    },
  },
  plugins: [],
};
export default config;
