import { describe, it, expect } from "vitest";

import { mount } from "@vue/test-utils";
import NavBar from "./Navbar.vue";

describe("NavBar", () => {
    it("renders properly", () => {
        const wrapper = mount(NavBar, {});

        expect(wrapper.text()).toContain("Home");
    });
});
