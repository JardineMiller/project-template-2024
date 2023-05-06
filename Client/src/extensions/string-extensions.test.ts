import { describe, it, expect } from "vitest";
import "./string-extensions";

describe("toTitleCase", () => {
    it.each(["", undefined])("handles bad input", (input) => {
        expect(input?.toTitleCase()).toBe(input);
    });

    it.each(["123", "1aaa"])(
        "handles numbers within strings",
        (input) => {
            expect(input?.toTitleCase()).toBe(input);
        }
    );

    it.each(["!Admin", "@Email"])(
        "handles special chars within strings",
        (input) => {
            expect(input?.toTitleCase()).toBe(input);
        }
    );

    it.each([
        ["admin", "Admin"],
        ["zebra", "Zebra"],
        ["bee", "Bee"],
        ["c", "C"],
    ])("handles single words", (input, expected) => {
        expect(input.toTitleCase()).toBe(expected);
    });

    it.each([
        ["the admin is here", "The Admin Is Here"],
        ["the zebra is happy", "The Zebra Is Happy"],
        ["the bumble bee", "The Bumble Bee"],
        ["c d e f g", "C D E F G"],
    ])("handles sentences words", (input, expected) => {
        expect(input.toTitleCase()).toBe(expected);
    });
});
