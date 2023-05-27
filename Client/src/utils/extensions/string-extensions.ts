export {};

declare global {
    interface String {
        toTitleCase(): string;
    }
}

String.prototype.toTitleCase = function (): string {
    if (!this || !this.length) {
        return this as string;
    }

    const capitalise = (word: string) => {
        return word.charAt(0).toUpperCase() + word.slice(1);
    };

    return this.split(" ")
        .map((word: string) => capitalise(word))
        .join(" ");
};
