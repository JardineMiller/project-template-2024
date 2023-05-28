import { describe, it, expect } from "vitest";

import { mount } from "@vue/test-utils";
import Register from "./Register.vue";

describe("Register", () => {
    it("renders properly", () => {
        const wrapper = mount(Register, {});

        expect(wrapper.text()).toContain("Register");
    });
});
