import type { Validator } from "@/models/Validator";

export default class FormInput {
    isValid: boolean = true;
    propertyName: string;
    validators: Array<Validator>;
    value: string;

    constructor(
        propertyName: string,
        initialValue: string = "",
        validators: Array<Validator> = []
    ) {
        this.propertyName = propertyName;
        this.validators = validators;
        this.value = initialValue;
    }
}
