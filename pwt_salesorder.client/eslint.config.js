import pluginJs from '@eslint/js';
import pluginVue from 'eslint-plugin-vue';
import globals from 'globals';
import tseslint from 'typescript-eslint';

/** @type {import('eslint').Linter.Config[]} */
export default [
	{ files: ['**/*.{js,mjs,cjs,ts,vue}'] },
	{ languageOptions: { globals: globals.browser } },
	pluginJs.configs.recommended,
	...tseslint.configs.recommended,
	...pluginVue.configs['flat/essential'],
	{ files: ['**/*.vue'], languageOptions: { parserOptions: { parser: tseslint.parser } } },
	{ rules: { '@typescript-eslint/no-unused-vars': 'warn', '@typescript-eslint/no-explicit-any': 'warn', 'vue/component-tags-order': ['error', { order: ['script', 'template', 'style'] }], 'vue/multi-word-component-names': 'off' } }
];
