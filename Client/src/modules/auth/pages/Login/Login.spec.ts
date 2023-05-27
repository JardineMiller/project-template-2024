import { describe, it, expect } from "vitest";

import LoginView from "@/pages/Login/Login.vue";
import { mount } from "@vue/test-utils";

describe("LoginView", () => {
    it("renders properly", () => {
        const wrapper = mount(LoginView, {});

        expect(wrapper.text()).toContain("Login");
    });
});
