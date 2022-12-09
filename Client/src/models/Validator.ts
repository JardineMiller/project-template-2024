export enum ValidatorType {
    required,
    minLength,
    maxLength,
    minNumber,
    maxNumber,
    email,
    pattern,
}

export class ValidationFnResult {
    isValid: boolean = true;
    errorMessage: string = "";

    constructor(isValid: boolean, errorMessage: string) {
        this.isValid = isValid;
        this.errorMessage = errorMessage;
    }
}

export interface Validator {
    type: ValidatorType;
    validate(input: string): ValidationFnResult;
}

export class Validators {
    static required(): Validator {
        return {
            type: ValidatorType.required,
            validate: (
                value: string,
                customMsg: string | null = null
            ): ValidationFnResult => {
                const isValid = value !== undefined && !!value.length;
                const msg = isValid
                    ? ""
                    : customMsg || "This field is required";

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static minLength(
        length: number,
        customMsg: string | null = null
    ): Validator {
        const min = length;

        return {
            type: ValidatorType.minLength,
            validate: (value: string): ValidationFnResult => {
                const isValid =
                    value !== undefined && value.length >= min;
                const msg = isValid
                    ? ""
                    : customMsg ||
                      `This field must be at least ${min} characters`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static maxLength(
        length: number,
        customMsg: string | null = null
    ): Validator {
        const max = length;

        return {
            type: ValidatorType.maxLength,
            validate: (value: string): ValidationFnResult => {
                const isValid =
                    value !== undefined && value.length <= max;
                const msg = isValid
                    ? ""
                    : customMsg ||
                      `This field must be no greater than ${max} characters`;

                return new ValidationFnResult(isValid, msg);
            },
        };
    }

    static email(): Validator {
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
    ): Validator {
        return {
            type: ValidatorType.minNumber,
            validate: (value: string): ValidationFnResult => {
                const exists = value !== undefined;
                const number = exists ? parseInt(value) : 0;

                const isValid = number >= min;
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
    ): Validator {
        return {
            type: ValidatorType.maxNumber,
            validate: (value: string): ValidationFnResult => {
                const exists = value !== undefined;
                const number = exists ? parseInt(value) : 0;

                const isValid = number <= max;
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
    ): Validator {
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
}
