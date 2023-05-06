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
    computed: {
        isRequired(): boolean {
            return this.modelValue.validators.some(
                (x) => x.type === ValidatorType.required
            );
        },
        isInvalid(): boolean {
            return (
                (Boolean(this.modelValue.value) ||
                    this.modelValue.touched) &&
                this.modelValue.errors.length > 0
            );
        },
    },
    methods: {
        onInput(value: string): void {
            this.$emit("onValueChange", {
                name: this.modelValue.propertyName,
                value: value,
            });
        },
        onBlur(): void {
            this.$emit("onBlur", {
                name: this.modelValue.propertyName,
            });
        },
    },
    emits: {
        onValueChange: null,
        onBlur: null,
    },
});
