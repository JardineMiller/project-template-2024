<template>
    <span class="p-float-label">
        <InputText
            :id="modelValue.propertyName.toLowerCase()"
            :name="modelValue.propertyName.toLowerCase()"
            :model-value="modelValue.value"
            :class="{
                'p-invalid': errors.length,
            }"
            @input="onInput($event.target.value)"
            @blur="onBlur($event.target.value)"
        />
        <label :for="modelValue.propertyName.toLowerCase()">
            {{ modelValue.propertyName.toTitleCase() }}
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
    import FormInputMixin from "@/mixins/forms/FormInputMixin";
    import "../../extensions/string-extensions";
    import InputText from "primevue/inputtext";
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "TextInput",
        components: { InputText },
        mixins: [FormInputMixin],
    });
</script>

<style scoped></style>
