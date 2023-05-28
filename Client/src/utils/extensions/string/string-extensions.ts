export {};

declare global {
    interface String {
        toTitleCase(): string;
        splitCamelCase(): string;
    }
}

String.prototype.toTitleCase = function (): string {
    if (!this || !this.length) {
        return this as string;
    }

    const capitalise = (word: string) => {
        return word.charAt(0).toUpperCase() + word.slice(1);
    };

    return this.splitCamelCase()
        .split(" ")
        .map((word: string) => capitalise(word))
        .join(" ");
};

String.prototype.splitCamelCase = function (): string {
    return this.replace(/([a-z0-9])([A-Z])/g, "$1 $2");
};
