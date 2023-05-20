import type { IValidator } from "@/models/validation/IValidator";
import { Validators } from "@/models/validation/Validators";

export class ModelPropertyChangeEvent<T> {
    readonly propertyName: string;
    readonly value: T;

    constructor(propertyName: string, value: T) {
        this.propertyName = propertyName;
        this.value = value;
    }
}

export class ModelPropertyEvent<T> {
    readonly propertyName: string;

    constructor(propertyName: string) {
        this.propertyName = propertyName;
    }
}

export default class ModelProperty<T> {
    value: T | undefined;
    propertyName: string;
    validators: Array<IValidator<T>>;
    touched: boolean = false;
    isRequired: boolean;

    constructor(
        propertyName: string,
        initialValue: T | undefined = undefined,
        isRequired: boolean = false,
        validators: Array<IValidator<T>> = []
    ) {
        this.propertyName = propertyName;
        this.validators = validators;
        this.value = initialValue;
        this.isRequired = isRequired;
    }

    get isValid(): boolean {
        if (this.isRequired) {
            const result = Validators.required().validate(this.value);

            if (!result.isValid) {
                return false;
            }
        }

        return this.validators.every(
            (x) => x.validate(this.value).isValid
        );
    }

    get errors(): string[] {
        if (this.isRequired) {
            const result = Validators.required().validate(this.value);

            if (!result.isValid) {
                return [result.errorMessage];
            }
        }

        return this.validators
            .map((x) => x.validate(this.value))
            .filter((x) => !x.isValid)
            .map((x) => x.errorMessage);
    }
    
    blurEvent(): ModelPropertyEvent<T> {
        return new ModelPropertyEvent<T>(this.propertyName);
    }

    changeEvent(val: T): ModelPropertyChangeEvent<T> {
        return new ModelPropertyChangeEvent<T>(
            this.propertyName,
            val
        );
    }

    touch(): void {
        this.touched = true;
    }
}
