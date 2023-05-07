import ModelProperty from "@/models/state/ModelProperty";
import { ValidatorType } from "@/models/IValidator";
import "../../extensions/string-extensions";
import { defineComponent } from "vue";

export default defineComponent({
    props: {
        modelValue: {
            type: ModelProperty,
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
            this.$emit(
                "onValueChange",
                this.modelValue.valueChangedEvent(value)
            );
        },
        onBlur(): void {
            this.$emit("onBlur", this.modelValue.blurEvent());
        },
    },
    emits: {
        onValueChange: null,
        onBlur: null,
    },
});
