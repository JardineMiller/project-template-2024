import { describe, it, expect } from "vitest";

import { mount } from "@vue/test-utils";
import LoginView from "./Login.vue";

describe("LoginView", () => {
    it("renders properly", () => {
        const wrapper = mount(LoginView, {});

        expect(wrapper.text()).toContain("Login");
    });
});
