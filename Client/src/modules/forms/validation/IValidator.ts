import type { ValidationFnResult } from "@/modules/forms/validation/ValidationFnResult";
import type { ValidatorType } from "@/modules/forms/validation/Validators";

export interface IValidator<T> {
    validate(input: T | undefined): ValidationFnResult;
    type: ValidatorType;
}
