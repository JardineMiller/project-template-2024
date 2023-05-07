import type { Validator } from "@/models/Validator";

export class PropertyValueChangedEvent {
    readonly propertyName: string;
    readonly value: string;

    constructor(propertyName: string, value: string) {
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

export default class ModelProperty {
    private _value: string;
    propertyName: string;
    validators: Array<Validator>;
    touched: boolean = false;

    constructor(
        propertyName: string,
        initialValue: string = "",
        validators: Array<Validator> = []
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

    get value(): string {
        return this._value;
    }

    set value(val: string) {
        this._value = val;
    }

    valueChangedEvent(val: string): PropertyValueChangedEvent {
        return new PropertyValueChangedEvent(this.propertyName, val);
    }

    blurEvent(): PropertyEvent {
        return new PropertyEvent(this.propertyName);
    }
}
