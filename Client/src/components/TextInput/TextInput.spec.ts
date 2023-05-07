import { describe, it, expect } from "vitest";

import type { IValidator } from "@/models/IValidator";
import TextInput from "./TextInput.vue";
import { mount } from "@vue/test-utils";

describe("TextInput", () => {
    it("renders properly", () => {
        const wrapper = mount(TextInput, {
            props: {
                name: "input-name",
                label: "input-label",
                modelValue: "input-value",
                validators: [] as Array<IValidator<string>>,
            },
        });
    });
});
