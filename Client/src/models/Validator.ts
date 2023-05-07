import { hasLengthProperty } from "@/utils/utils";

export enum ValidatorType {
    required,
    minLength,
    maxLength,
    minNumber,
    maxNumber,
    email,
    pattern,
    custom,
}

export class ValidationFnResult {
    isValid: boolean = true;
    errorMessage: string = "";

    constructor(isValid: boolean, errorMessage: string) {
        this.isValid = isValid;
        this.errorMessage = errorMessage;
    }
}

export interface IValidator<T> {
    type: ValidatorType;
    validate(input: T | undefined): ValidationFnResult;
}

export class Validators {
    static required<T>(): IValidator<T> {
        return {
            type: ValidatorType.required,
            validate: (
                value: T,
                customMsg: string | null = null
            ): ValidationFnResult => {
                const exists = value !== undefined;
                const hasLength = hasLengthProperty(value)
                    ? value.length > 0
                    : false;

                const isValid = exists && hasLength;

                const msg = isValid
                    ? ""
                    : customMsg || "This field is required";

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static minLength<T>(
        length: number,
        customMsg: string | null = null
    ): IValidator<T> {
        const min = length;

        return {
            type: ValidatorType.minLength,
            validate: (value: T): ValidationFnResult => {
                const exists = value !== undefined;
                const hasLength = hasLengthProperty(value)
                    ? value.length >= min
                    : false;

                const isValid = exists && hasLength;

                const msg = isValid
                    ? ""
                    : customMsg ||
                      `This field must be at least ${min} characters`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static maxLength<T>(
        length: number,
        customMsg: string | null = null
    ): IValidator<T> {
        const max = length;

        return {
            type: ValidatorType.maxLength,
            validate: (value: T): ValidationFnResult => {
                const exists = value !== undefined;
                const hasLength = hasLengthProperty(value)
                    ? value.length >= max
                    : false;

                const isValid = exists && hasLength;

                const msg = isValid
                    ? ""
                    : customMsg ||
                      `This field must be no greater than ${max} characters`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static email(): IValidator<string> {
        return {
            type: ValidatorType.email,
            validate: (
                value: string,
                customMsg: string | null = null
            ): ValidationFnResult => {
                const isValid =
                    value !== undefined &&
                    /^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/.test(value);

                const msg = isValid
                    ? ""
                    : customMsg ||
                      "This field must contain a valid email address";

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static minNumber(
        min: number,
        customMsg: string | null = null
    ): IValidator<number> {
        return {
            type: ValidatorType.minNumber,
            validate: (value: number): ValidationFnResult => {
                const exists = value !== undefined;
                const isValid = exists && value >= min;

                const msg = isValid
                    ? ""
                    : customMsg || `Minimum value: ${min}`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static maxNumber(
        max: number,
        customMsg: string | null = null
    ): IValidator<number> {
        return {
            type: ValidatorType.maxNumber,
            validate: (value: number): ValidationFnResult => {
                const exists = value !== undefined;
                const isValid = exists && value <= max;

                const msg = isValid
                    ? ""
                    : customMsg || `Maximum value: ${max}`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static pattern(
        regex: RegExp,
        customMsg: string | null = null
    ): IValidator<string> {
        return {
            type: ValidatorType.pattern,
            validate: (value: string): ValidationFnResult => {
                const exists = value !== undefined;
                const isValid = exists && regex.test(value);

                const msg = isValid
                    ? ""
                    : customMsg ||
                      "This field does not match the required pattern";

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static custom<T>(
        isValidCallback: (input: T) => boolean,
        customMsg: string | null = null
    ): IValidator<T> {
        return {
            type: ValidatorType.custom,
            validate: (value: T): ValidationFnResult => {
                const isValid = isValidCallback(value);

                const msg = isValid
                    ? ""
                    : customMsg || "This field is invalid";

                return new ValidationFnResult(isValid, msg);
            },
        };
    }
}
