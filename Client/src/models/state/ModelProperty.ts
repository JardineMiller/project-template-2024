import type { IValidator } from "@/models/IValidator";

export class PropertyValueChangedEvent<T> {
    readonly propertyName: string;
    readonly value: T;

    constructor(propertyName: string, value: T) {
        this.propertyName = propertyName;
        this.value = value;
    }
}

export class PropertyEvent {
    readonly propertyName: string;

    constructor(propertyName: string) {
        this.propertyName = propertyName;
    }
}

export default class ModelProperty<T> {
    private _value: T;
    propertyName: string;
    validators: Array<IValidator<T>>;
    touched: boolean = false;

    constructor(
        propertyName: string,
        initialValue: T,
        validators: Array<IValidator<T>> = []
    ) {
        this.propertyName = propertyName;
        this.validators = validators;
        this._value = initialValue;
    }

    get isValid(): boolean {
        return this.validators.every(
            (x) => x.validate(this.value).isValid
        );
    }

    get errors(): string[] {
        return this.validators
            .map((x) => x.validate(this.value))
            .filter((x) => !x.isValid)
            .map((x) => x.errorMessage);
    }

    get value(): T {
        return this._value;
    }

    set value(val: T) {
        this._value = val;
    }

    valueChangedEvent(val: T): PropertyValueChangedEvent<T> {
        return new PropertyValueChangedEvent<T>(
            this.propertyName,
            val
        );
    }

    blurEvent(): PropertyEvent {
        return new PropertyEvent(this.propertyName);
    }
}
