import { describe, it, expect } from "vitest";

import ModelProperty from "@/models/state/ModelProperty";
import TextInput from "./TextInput.vue";
import { mount } from "@vue/test-utils";

describe("TextInput", () => {
    it("renders properly", () => {
        const wrapper = mount(TextInput, {
            props: {
                modelValue: new ModelProperty<string>("name", ""),
            },
        });
    });
});
