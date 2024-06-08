/* eslint-env node */
require("@rushstack/eslint-patch/modern-module-resolution");

module.exports = {
    root: true,
    extends: [
        "plugin:vue/vue3-essential",
        "eslint:recommended",
        "@vue/eslint-config-typescript",
        // "@vue/eslint-config-prettier",
    ],
    overrides: [
        {
            files: ["playwright/**/*.{spec}.{js,ts,jsx,tsx}"],
            extends: ["plugin:playwright/recommended"],
        },
    ],
    parserOptions: {
        ecmaVersion: "latest",
    },
    rules: {
        "vue/multi-word-component-names": "off",
        "vue/no-reserved-component-names": "off",
    },
};
