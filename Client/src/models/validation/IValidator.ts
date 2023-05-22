import type { ValidationFnResult } from "@/models/validation/ValidationFnResult";
import type { ValidatorType } from "@/models/validation/Validators";

export interface IValidator<T> {
    validate(input: T | undefined): ValidationFnResult;
    type: ValidatorType;
}
