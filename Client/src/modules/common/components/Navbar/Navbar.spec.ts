import { createRouter, createWebHistory } from "vue-router";
import { describe, it, expect } from "vitest";
import { routes } from "@/services/router"; // This import should point to your routes file declared above
import { mount } from "@vue/test-utils";
import NavBar from "./Navbar.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: routes,
});

describe("NavBar", () => {
    it("renders properly", () => {
        const wrapper = mount(NavBar, {
            global: {
                plugins: [router],
            },
        });

        expect(
            wrapper.find('[data-test-id="navbar-link-home"]').text()
        ).toContain("Home");

        expect(
            wrapper.find('[data-test-id="navbar-link-login"]').text()
        ).toContain("Login");
    });
});
