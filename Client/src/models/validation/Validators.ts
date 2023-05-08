import { ValidationFnResult } from "@/models/validation/ValidationFnResult";
import { ValidatorType } from "@/models/validation/ValidatorType";
import type { IValidator } from "@/models/validation/IValidator";
import { EMAIL_REGEX, hasLengthProperty } from "@/utils/utils";

export class Validators {
    private static exists<T>(val: T | undefined | null): boolean {
        return val !== undefined && val !== null;
    }

    private static hasLength<T>(
        val: T,
        predicate: (length: number) => boolean
    ): boolean {
        return hasLengthProperty(val) ? predicate(val.length) : false;
    }

    private static simpleReduce<T>(value: T) {
        return (
            accumulatedValue: boolean,
            currentFn: (value: T) => boolean
        ) => {
            return accumulatedValue && currentFn(value);
        };
    }

    private static predicateReduce<T, K>(
        value: T,
        predicate: (input: K) => boolean
    ) {
        return (
            accumulatedValue: boolean,
            currentFn: (
                value: T,
                predicate: (input: K) => boolean
            ) => boolean
        ) => {
            return accumulatedValue && currentFn(value, predicate);
        };
    }

    static required<T>(): IValidator<T> {
        return {
            type: ValidatorType.required,
            validate: (
                value: T,
                customMsg: string | null = null
            ): ValidationFnResult => {
                const conditions = [this.exists, this.hasLength];

                const isValid = conditions.reduce(
                    this.predicateReduce(
                        value,
                        (valueLength) => valueLength > 0
                    ),
                    true
                );

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
                const conditions = [this.exists, this.hasLength];

                const isValid = conditions.reduce(
                    this.predicateReduce(
                        value,
                        (valueLength) => valueLength >= min
                    ),
                    true
                );

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
                const conditions = [this.exists, this.hasLength];

                const isValid = conditions.reduce(
                    this.predicateReduce(
                        value,
                        (valueLength) => valueLength <= max
                    ),
                    true
                );

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
                const conditions = [
                    this.exists,
                    EMAIL_REGEX.test.bind(EMAIL_REGEX),
                ];

                const isValid = conditions.reduce(
                    this.simpleReduce(value),
                    true
                );

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
                const conditions = [
                    this.exists,
                    (value: number) => value >= min,
                ];

                const isValid = conditions.reduce(
                    this.simpleReduce(value),
                    true
                );

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
                const conditions = [
                    this.exists,
                    (value: number) => value <= max,
                ];

                const isValid = conditions.reduce(
                    this.simpleReduce(value),
                    true
                );

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
                const conditions = [
                    this.exists,
                    regex.test.bind(regex),
                ];

                const isValid = conditions.reduce(
                    this.simpleReduce(value),
                    true
                );

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
