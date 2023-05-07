export function hasLengthProperty(
    obj: any
): obj is { length: number } {
    return typeof obj.length === "number";
}
