import ModelProperty from "@/models/state/ModelProperty";
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
                "onChange",
                this.modelValue.changeEvent(value)
            );
        },
        onBlur(): void {
            this.$emit("onBlur", this.modelValue.blurEvent());
        },
    },
    emits: {
        onChange: null,
        onBlur: null,
    },
});
