<template>
    <span class="p-float-label">
        <InputText
            :id="name.toLowerCase()"
            :name="name.toLowerCase()"
            :model-value="modelValue"
            :class="{
                'p-invalid': errors.length,
            }"
            @input="onInput($event.target.value)"
            @blur="onBlur($event.target.value)"
        />
        <label for="name">
            {{ label.toTitleCase() }}
            <span
                v-if="isRequired"
                class="p-error"
            >
                *
            </span>
        </label>
    </span>
    <small
        v-for="error in errors"
        :key="error"
        class="p-error"
    >
        {{ error }} <br />
    </small>
</template>

<script lang="ts">
    import type { Validator } from "@/models/Validator";
    import { ValidatorType } from "@/models/Validator";
    import "../../extensions/string-extensions";
    import InputText from "primevue/inputtext";
    import { defineComponent } from "vue";
    import type { PropType } from "vue";

    export default defineComponent({
        name: "TextInput",
        components: { InputText },
        props: {
            name: {
                type: String,
                required: true,
            },
            label: {
                type: String,
                required: true,
            },
            modelValue: {
                type: String,
                required: true,
            },
            validators: {
                type: Array as PropType<Array<Validator>>,
                default() {
                    return [] as Array<Validator>;
                },
            },
        },
        data() {
            return {
                errors: [] as string[],
            };
        },
        computed: {
            isRequired(): boolean {
                return this.validators.some(
                    (x) => x.type === ValidatorType.required
                );
            },
        },
        methods: {
            isValid(value: string): boolean {
                return this.validators?.every(
                    (x) => x.validate(value).isValid
                );
            },
            populateErrors(value: string): void {
                this.errors = [];
                for (const validator of this.validators) {
                    const validationResult =
                        validator.validate(value);
                    if (!validationResult.isValid) {
                        this.errors.push(
                            validationResult.errorMessage
                        );
                    }
                }
            },
            onInput(value: string): void {
                const isValid = this.isValid(value);
                this.populateErrors(value);
                this.$emit("updateIsValid", {
                    name: this.label,
                    value: isValid,
                });
                this.$emit("updateValue", {
                    name: this.label,
                    value: value,
                });
            },
            onBlur(value: string): void {
                const isValid = this.isValid(value);
                this.populateErrors(value);
                this.$emit("updateIsValid", {
                    name: this.label,
                    value: isValid,
                });
            },
        },
        emits: {
            updateValue: null,
            updateIsValid: null,
        },
        beforeMount() {
            const isValid = this.isValid(this.modelValue);
            this.$emit("updateIsValid", {
                name: this.label,
                value: isValid,
            });
        },
    });
</script>

<style scoped></style>
