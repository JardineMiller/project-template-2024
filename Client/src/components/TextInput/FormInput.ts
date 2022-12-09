import { describe, it, expect } from "vitest";

import TextInput from "./TextInput.vue";
import { mount } from "@vue/test-utils";

describe("TextInput", () => {
    it("renders properly", () => {
        const wrapper = mount(TextInput, {});

        expect(wrapper.text()).toContain("");
    });
});
