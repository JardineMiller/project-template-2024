import { ValidatorType } from "@/models/Validator";
import FormInput from "@/models/forms/FormInput";
import "../../extensions/string-extensions";
import { defineComponent } from "vue";

export default defineComponent({
    props: {
        modelValue: {
            type: FormInput,
            required: true,
        },
    },
    data() {
        return {
            errors: [] as string[],
        };
    },
    computed: {
        isRequired(): boolean {
            return this.modelValue.validators.some(
                (x) => x.type === ValidatorType.required
            );
        },
    },
    methods: {
        isValid(value: string): boolean {
            return this.modelValue.validators?.every(
                (x) => x.validate(value).isValid
            );
        },
        populateErrors(value: string): void {
            this.errors = [];
            for (const validator of this.modelValue.validators) {
                const validationResult = validator.validate(value);
                if (!validationResult.isValid) {
                    this.errors.push(validationResult.errorMessage);
                }
            }
        },
        onInput(value: string): void {
            const isValid = this.isValid(value);
            this.populateErrors(value);

            this.$emit("updateValue", {
                name: this.modelValue.propertyName,
                value: value,
                isValid: isValid,
            });
        },
        onBlur(value: string): void {
            const isValid = this.isValid(value);
            this.populateErrors(value);

            this.$emit("updateValue", {
                name: this.modelValue.propertyName,
                value: value,
                isValid: isValid,
            });
        },
    },
    emits: {
        updateValue: null,
        updateIsValid: null,
    },
    beforeMount() {
        const isValid = this.isValid(this.modelValue.value);

        this.$emit("updateValue", {
            name: this.modelValue.propertyName,
            value: this.modelValue.value,
            isValid: isValid,
        });
    },
});
