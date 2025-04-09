import type { Config } from 'tailwindcss';
import tailwindcssPrimeui from 'tailwindcss-primeui';

const config: Config = {
	darkMode: ['selector', '[class*="app-dark"]'],
	content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
	plugins: [tailwindcssPrimeui],
	theme: {
		screens: {
			sm: '576px',
			md: '768px',
			lg: '992px',
			xl: '1200px',
			'2xl': '1920px'
		}
	}
};

export default config;
