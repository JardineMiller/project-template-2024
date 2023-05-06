import type { Validator } from "@/models/Validator";

export default class FormInput {
    propertyName: string;
    validators: Array<Validator>;
    value: string;

    touched: boolean = false;

    constructor(
        propertyName: string,
        initialValue: string = "",
        validators: Array<Validator> = []
    ) {
        this.propertyName = propertyName;
        this.validators = validators;
        this.value = initialValue;
    }

    get isValid(): boolean {
        return this.validators?.every(
            (x) => x.validate(this.value).isValid
        );
    }

    get errors(): string[] {
        return this.validators
            .map((x) => x.validate(this.value))
            .filter((x) => !x.isValid)
            .map((x) => x.errorMessage);
    }
}
