<template>
    <span class="p-float-label">
        <InputText
            :id="modelValue.propertyName.toLowerCase()"
            :name="modelValue.propertyName.toLowerCase()"
            :model-value="modelValue.value"
            :class="{
                'p-invalid': isInvalid,
            }"
            @input="onInput($event.target.value)"
            @blur="onBlur()"
        />
        <label :for="modelValue.propertyName.toLowerCase()">
            {{ modelValue.propertyName.toTitleCase() }}
            <span
                v-if="modelValue.isRequired"
                class="p-error"
            >
                *
            </span>
        </label>
    </span>
    <div v-if="isInvalid">
        <small
            v-for="error in modelValue.errors"
            :key="error"
            class="p-error"
        >
            {{ error }} <br />
        </small>
    </div>
</template>

<script lang="ts">
    import { ValidatorType } from "@/models/validation/ValidatorType";
    import ModelProperty from "@/models/state/ModelProperty";
    import "../../extensions/string-extensions";
    import InputText from "primevue/inputtext";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "TextInput",
        components: { InputText },
        props: {
            modelValue: {
                type: ModelProperty<string>,
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
</script>

<style scoped></style>
