import type { ValidationFnResult } from "@/models/validation/ValidationFnResult";
import type { ValidatorType } from "@/models/validation/ValidatorType";

export interface IValidator<T> {
    type: ValidatorType;
    validate(input: T | undefined): ValidationFnResult;
}
