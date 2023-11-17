export function hasLengthProperty(
    obj: any
): obj is { length: number } {
    return typeof obj.length === "number";
}
export const EMAIL_REGEX = /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/;
