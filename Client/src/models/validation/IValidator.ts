import type { ValidationFnResult } from "@/models/validation/ValidationFnResult";

export interface IValidator<T> {
    validate(input: T | undefined): ValidationFnResult;
}
